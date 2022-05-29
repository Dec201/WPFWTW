using System.Text;
using ChoETL;
using WPFWTW.Model;
using WPFWTW.Logger;
using WPFWTW.Utility;
using WPFWTW.Persistence;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace WPFWTW.Data
{
    public class CSVDataMapper
    {

        private int lowestOriginDate;
        private int findLatestDevYear;
        private int developmentYearsRange;
        private List<ClaimsDataModel> claimsDataModels = new List<ClaimsDataModel>();

        private readonly ILogger _logger;
        private readonly IPersistence _persistence;
        private readonly string _filePath;


        public CSVDataMapper(ILogger logger, IPersistence persistence, string filepath)
        {
            _logger = logger;
            _persistence = persistence;
            _filePath = filepath;
        }

        public void MapCSVData()
        {

            _logger.LogInfo(InfoCodes.Info_0100, _filePath);

            FilePathChecker();
            ExtractData();

            if(claimsDataModels.Count != 0)
            {
                InitialDataCollection();
                MainDataCollection();
            }
            else
            {
                _logger.LogInfo(InfoCodes.Info_0300, _filePath);
            }

            _logger.LogInfo(InfoCodes.Info_0200, _filePath);

        }


        private void FilePathChecker()
        {
            // File path check
            if (!File.Exists(_filePath))
            {
                _logger.LogError(ErrorCodes.Error_0201, _filePath);
            }
        }


        private void ExtractData()
        {
            // Reading data from CSV + add to private list
            // using ChoETL package, Model - ClaimsDataModel
            try
            {
                using (var stream = File.Open(_filePath, FileMode.Open))
                {
                    foreach (var item in new ChoCSVReader<ClaimsDataModel>(stream))
                    {
                        claimsDataModels.Add(item);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), _filePath);
                _logger.LogError(ErrorCodes.Error_0200, _filePath);
            }
        }


        private void InitialDataCollection()
        {
            // Min origin date + Max dev date from the origin, data might miss first dev year so need to take from origin year
            lowestOriginDate = claimsDataModels.Min(x => x.OriginYear);
            findLatestDevYear = (from x in claimsDataModels where x.OriginYear == lowestOriginDate select x.DevelopmentYear).Max();
            developmentYearsRange = (findLatestDevYear - lowestOriginDate) + 1;

            // collecting and saving first line data
            string intialData = lowestOriginDate + ", " + developmentYearsRange;
            SaveData(intialData);
        }


        private void MainDataCollection()
        {

            // select all different products and origin years
            var differentProductNames = claimsDataModels.Select(x => x.Product).Distinct();
            var distinctOriginYears = claimsDataModels.Select(x => x.OriginYear).Distinct().OrderBy(x => x);

            // loop depends on number of different products
            foreach (var product in differentProductNames)
            {

                StringBuilder currentProductCumulative = new StringBuilder();
                currentProductCumulative.Append(product);
                float cumulativeValue = 0;

                // loop through origin years
                for (int currentOriginYear = distinctOriginYears.Min(); currentOriginYear < (distinctOriginYears.Max() + 1); currentOriginYear++)
                {

                    // find all development years within the set current origin year
                    var findDevYearMax = (from x in claimsDataModels where x.OriginYear == currentOriginYear select x.DevelopmentYear).Max();
                    var findDevYearMin = (from x in claimsDataModels where x.OriginYear == currentOriginYear select x.DevelopmentYear).Min();

                    //  loop through development year range
                    for (int currentDevYear = findDevYearMin; currentDevYear < findDevYearMax + 1; currentDevYear++)
                    {

                        var findMatchingClaim = claimsDataModels.Find(x => (x.Product == product) && (x.OriginYear == currentOriginYear) && (x.DevelopmentYear == currentDevYear));

                        if (findMatchingClaim != null)
                        {
                            cumulativeValue += findMatchingClaim.IncrementalValue;
                        }
                        else
                        {
                            cumulativeValue += 0;
                        }

                        currentProductCumulative.Append(", " + cumulativeValue);

                    }

                    cumulativeValue = 0;

                }

                SaveData(currentProductCumulative.ToString());

            }

            
            
        }


        private void SaveData(string data)
        {
            // Attempt to save to file
            try
            {
                _persistence.Save(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), _filePath);
                _logger.LogError(ErrorCodes.Error_0400, _filePath);
            }
        }





    }
}
