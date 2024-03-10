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
	public class Core
	{
		//TODO: Ensure Consistent Versioning of Selenium Packages to avoid build errors
		public void Option(string selection, string element, IWebDriver localDriver)
		{
			if (IsElementPresent(element, localDriver) == true)
                        {
				SelectElement e = new SelectElement(localDriver.FindElement(By.CssSelector(element)));
				Thread.Sleep(2000);
				e.SelectByText(selection);
				Thread.Sleep(1000);
			}
		}

		public void Wait(string element, IWebDriver localDriver)
		{
			var attempts = 0;
			var maxAttempts = 5;
			while (attempts < maxAttempts)
			{
				if (IsElementPresent(element, localDriver) == false)
				{
					Thread.Sleep(4000); 
					attempts++;
					Console.WriteLine(attempts);
					continue;
				}
				else
				{
					break;
				}
			}
			Console.WriteLine("tried to find element: " + element + " " + attempts + " times");
		}

		//must use LOCAL driver within functionse regular 'driver' will flag as not set to reference
		//returns the present text within a specific element
		public string GetText(string element, IWebDriver localDriver)
		{
			if (IsElementPresent(element, localDriver) == true)
			{
				var selectedElement = localDriver.FindElement(By.CssSelector(element));
				if (selectedElement.Text != "")
				{
					return selectedElement.Text;
				}
				else //readonly input case
				{
					return selectedElement.GetAttribute("value");
				}
			}
			else
			{
				return "element not found: " +  element;
			}
		}

		//function for text box entry selection and entry
		public void EnterText(string str, string element, IWebDriver localDriver)
		{
			if (IsElementPresent(element, localDriver) == true)
			{
				Click(element, localDriver);
				localDriver.FindElement(By.CssSelector(element)).SendKeys(str);
				Thread.Sleep(300);
			}
		}


		
		//important for pages that remove elements dynamically
                //allows you to check for its existence on page
		public bool IsElementPresent(string element, IWebDriver localDriver)//pass in any find element by css-selector
		{
		    try                                 //attempts to find the element
		    {
			localDriver.FindElement(By.CssSelector(element));
			return true;
		    }
		    catch (NoSuchElementException)      //catches exception if not found
		    {
			Console.WriteLine("Element Not on Page");
			return false;
		    }
		}

		//checks for a specific string within the entirety of an element including nested sub-elements
		//useful for making sure that a certain labeled button, phrase, word, or title has been removed or isn't present
		public bool HasStr(string str, string element, IWebDriver localDriver)
		{
			try
			{
				if (IsElementPresent(element, localDriver) == true)
				{	
					string text = localDriver.FindElement(By.CssSelector(element)).Text;
					if (text.Contains(str) == false)
					{
						return false;
					}
					return true;
				}
				else
				{
					return false;
				}
			}
			catch 
			{
				Console.WriteLine("HasStr caught error");
				return false;
			}
		}

		//Checks the element's border selector(if it exists, returns false if not), checks the px size is greater than 0
		//checks if the border selector contains the keyword 'none'
		public bool HasBorder(string element, IWebDriver localDriver)
		{
			try
			{
				if (IsElementPresent(element, localDriver) == true)
                                {
					var borderWidth = 0;
					var border = localDriver.FindElement(By.CssSelector(element)).GetCssValue("border");
					if (border.Contains("px"))
					{
						var index = border.IndexOf("px");
						if (index == 1)
						{
							borderWidth = border[0];
						}
						if (index > 1)
						{
							var i = 0;
							var bw = "";
							while (i < index)
							{
								bw.Append(border[i]);
								i++;
							}
							borderWidth = Int16.Parse(bw);
						}
					}
					if (border.Contains("none"))
					{
						return false;
					}
					if (borderWidth < 1)
					{
						return false;
					}
					//can also check for transparent potentially
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				Console.WriteLine("No border, border px < 1, or border value none");
				return false;
			}
		}
		
		//Shortened version of the selenium Click() function
		public void Click(string element, IWebDriver localDriver)
		{
			if (IsElementPresent(element, localDriver) == true)
			{
				try
				{//can put wait and IsElementPresent before this to prevent errors
				 //can also modularize the selector method if needed
					
					localDriver.FindElement(By.CssSelector(element)).Click();
				}
				catch
				{
					string err = "Cant be clicked, doesnt exist, or is using 'By' with non-css selector";
					Console.WriteLine(err);
				}
			}
		}


		public IWebDriver driver;

		public Dictionary<int, Size> screenSizes = new Dictionary<int, Size>()
		{
			{1080, new Size(1920, 1080)},
			{1440, new Size(2560, 1440)}
		};

	

	}
}
