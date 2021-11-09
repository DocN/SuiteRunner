using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SuiteRunner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentBtn = (Button)sender;
            var resultsBox = (TextBox)this.FindName("resultsBox");
            try
            {
                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("headless");
                chromeDriverService.HideCommandPromptWindow = true;
                if (currentBtn != null && currentBtn.Name == "Search")
                {
                    var driver = new OpenQA.Selenium.Chrome.ChromeDriver(chromeDriverService, chromeOptions);
                    driver.Navigate().GoToUrl("https://www.bcassessment.ca/");
                    var input = driver.FindElement(By.Id("rsbSearch"));
                    var searchInput = (TextBox)this.FindName("searchBox");
                    if (searchInput != null)
                    {
                        //5900 Alderbridge Way
                        input.SendKeys(searchInput.Text);
                        System.Threading.Thread.Sleep(1000);
                        var autofills = driver.FindElement(By.ClassName("ui-menu-item"));
                        IWebElement autofillResult = null;
                        if (autofills != null)
                        {
                            autofillResult = autofills.FindElement(By.ClassName("ui-menu-item-wrapper"));
                        }
                        if (autofillResult != null)
                        {
                            autofillResult.Click();
                            System.Threading.Thread.Sleep(1000);
                            var modalDD = driver.FindElement(By.Id("ddlSubUnits"));
                            if (modalDD != null)
                            {
                                resultsBox.Clear();
                                var options = modalDD.FindElements(By.TagName("option"));
                                foreach (var option in options)
                                {
                                    if (option.Text == "Select unit")
                                    {
                                        continue;
                                    }
                                    Trace.WriteLine(option.Text);
                                    resultsBox.Text = resultsBox.Text + option.Text + "\n";
                                }
                            }
                        }
                    }
                }
            } catch(Exception error)
            {
                resultsBox.Text = error.Message;
            }
        }
    }
}
