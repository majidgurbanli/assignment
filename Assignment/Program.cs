using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! Give csvFilePath: ");
            var csvFilePath = Console.ReadLine();
            //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //string csvFilePath = "C:\\Users\\Majid\\Downloads\\sample-cab-data.csv"; // Replace with the actual path of your CSV file

            List<Record> records = ReadOperations.ReadCsvFile(csvFilePath);

            // Now 'records' contains the data from the CSV file
            List<Record> uniqueRecords = ReadOperations.IdentifyAndRemoveDuplicates(records);

            // Convert 'N' values to 'No' and 'Y' values to 'Yes' in store_and_fwd_flag column
            foreach (var record in records)
            {
                record.StoreAndForwardFlag = ReadOperations.ConvertStoreAndFwdFlag(record.StoreAndForwardFlag);
            }

            // Ensure that all text-based fields are free from leading or trailing whitespace
            foreach (var record in records)
            {
                record.StoreAndForwardFlag = record.StoreAndForwardFlag?.Trim();
                
            }

            ReadOperations.SaveDuplicates(records.Except(uniqueRecords).ToList(), "duplicates.csv");
            DatabaseOperations.InsertBulkRecords(records);
        }
    }
}
