using CsvHelper;
using System.Globalization;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System;
using System.Drawing;
namespace GenericSeleniumTool
{
	public class Case //generic example
	{
		public string Key1 {get; set;}
		public string Key2 {get; set;}
	}

        public class CSV_Parsing
        {
		Subroutines sr = new Subroutines();

		public void CSV_Parse(string path, IWebDriver driver)
		{
			using (var reader = new StreamReader(path))
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                                csv.Read();
                                csv.ReadHeader();
                                while (csv.Read())
                                {
                                        var record = csv.GetRecord<Case>();
                                        String[] currentCase = new String[]
                                        {
                                                record.Key1,
                                                record.Key2,
                                        };
                                        List<string> entries = currentCase.ToList<string>();
                                        driver.Navigate().Refresh();
                                }
                        }
		}
	}
}
