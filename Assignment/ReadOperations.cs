using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Assignment
{
    internal class ReadOperations
    {
       static public List<Record> ReadCsvFile(string filePath)
        {
            List<Record> records = new List<Record>();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                string[] headers = parser.ReadFields();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    Record record = MapToRecord(headers, fields);
                    records.Add(record);
                }
            }


            return records;
        }


        static public Record MapToRecord(string[] headers, string[] fields)
        {
            Record record = new Record();

            for (int i = 0; i < headers.Length; i++)
            {
                // Check for empty or null values before parsing
                if (string.IsNullOrWhiteSpace(fields[i]))
                {
                    continue;
                }

              
                switch (headers[i])
                {
                    case "tpep_pickup_datetime":
                        record.PickupDateTime = ConvertESTtoUTC(DateTime.Parse(fields[i]));
                        break;
                    case "tpep_dropoff_datetime":
                        record.DropoffDateTime = ConvertESTtoUTC(DateTime.Parse(fields[i]));
                        break;
                    case "passenger_count":
                        record.PassengerCount = int.Parse(fields[i]);
                        break;
                    case "trip_distance":
                        record.TripDistance = double.Parse(fields[i]);
                        break;
                    case "store_and_fwd_flag":
                        record.StoreAndForwardFlag = fields[i];
                        break;
                    case "PULocationID":
                        record.PULocationID = int.Parse(fields[i]);
                        break;
                    case "DOLocationID":
                        record.DOLocationID = int.Parse(fields[i]);
                        break;
                    case "fare_amount":
                        record.FareAmount = decimal.Parse(fields[i]);
                        break;
                    case "tip_amount":
                        record.TipAmount = decimal.Parse(fields[i]);
                        break;
                      
                }
            }

            return record;
        }

        static public DateTime ConvertESTtoUTC(DateTime dateTime)
        {
            TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, estTimeZone);
        }

        static public  List<Record> IdentifyAndRemoveDuplicates(List<Record> records)
        {
            var uniqueRecords = new List<Record>();
            var duplicateRecords = new List<Record>();
            var seenKeys = new HashSet<(DateTime, DateTime, int)>();

            foreach (var record in records)
            {
                var key = (record.PickupDateTime, record.DropoffDateTime, record.PassengerCount);

                if (seenKeys.Add(key))
                {
                    uniqueRecords.Add(record);
                }
                else
                {
                    duplicateRecords.Add(record);
                }
            }

            // Save duplicate records to a duplicates.csv file
            SaveDuplicates(duplicateRecords, "duplicates.csv");

            return uniqueRecords;

           
        }

        static public void SaveDuplicates(List<Record> duplicates, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                // Write headers
                writer.WriteLine("tpep_pickup_datetime,tpep_dropoff_datetime,passenger_count");

                // Write duplicate records
                foreach (var record in duplicates)
                {
                    writer.WriteLine($"{record.PickupDateTime},{record.DropoffDateTime},{record.PassengerCount}");
                }
            }
        }

        static public string ConvertStoreAndFwdFlag(string value)
        {
            // Convert 'N' values to 'No' and 'Y' values to 'Yes'
            if (string.Equals(value, "N", StringComparison.OrdinalIgnoreCase))
            {
                return "No";
            }
            else if (string.Equals(value, "Y", StringComparison.OrdinalIgnoreCase))
            {
                return "Yes";
            }
            else
            {
                return value; // Keep the value unchanged if not 'N' or 'Y'
            }
        }
    }

}
