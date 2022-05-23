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
            Console.WriteLine(lowestOriginDate + ", " + developmentYearsRange);
            string intialData = lowestOriginDate + ", " + developmentYearsRange;
            SaveData(intialData);
        }


        private void MainDataCollection()
        {

            /*
             Current Logic Incorrect - Does not equate for null origin/dev years

            loop through from origin year min - find latest dev year with origin year - count different
            loop through difference increment'+1 onto dev year if dev year == null stick with current cumlativeValue
            if dev/origin year both null, result - 0
             */

            // select all different products
            var differentProductNames = claimsDataModels.Select(x => x.Product).Distinct();
            float cumulativeValue = 0;

            // loop depends on number of different products
            foreach (var product in differentProductNames)
            {

                IEnumerable<ClaimsDataModel> separateProductList = claimsDataModels.Where(x => x.Product == product);

                StringBuilder currentProductCumulative = new StringBuilder();
                currentProductCumulative.Append(product);

                foreach (var item in separateProductList)
                {

                    if (item.DevelopmentYear == item.OriginYear)
                    {
                        cumulativeValue = item.IncrementalValue;
                    }

                    if (item.DevelopmentYear != item.OriginYear)
                    {
                        cumulativeValue += item.IncrementalValue;
                    }

                    currentProductCumulative.Append(", " + cumulativeValue);

                }


                Console.WriteLine(currentProductCumulative);
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
