﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace PolandVisa
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            //Хмельницький - 15
            //Вінниця - 15

            var task1 = new Task(() => CheckVisaDays("15", this));
            task1.Start();

            var task2 = new Task(() => CheckVisaDays("17", this));
            task2.Start();
        }

        private static void CheckVisaDays(string district, Form main)
        {
            var player = new System.Media.SoundPlayer();
            player.SoundLocation = @"C:\Users\odu\Music\strings.wav";

            var driver = new FirefoxDriver();
            do
            {
                driver.Navigate().GoToUrl("http://polandvisa-ukraine.com/scheduleappointment_2.html");
                driver.SwitchTo().Frame(0);

                new WebDriverWait(driver, TimeSpan.FromDays(1)).Until(
                    drv =>
                    {
                        try
                        {
                            return drv.FindElement(By.Id("ctl00_plhMain_lnkSchApp"));
                        }
                        catch (NoSuchElementException)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(5));
                            drv.Navigate().Refresh();
                            throw;
                        }
                    }).Click();

                driver.SwitchTo().DefaultContent();
                driver.SwitchTo().Frame(0);

                new SelectElement(new WebDriverWait(driver, TimeSpan.FromMinutes(10)).Until(drv => driver.FindElement(By.Id("ctl00_plhMain_cboVAC")))).SelectByValue(district);

                driver.SwitchTo().DefaultContent();
                driver.SwitchTo().Frame(0);
                new SelectElement(driver.FindElement(By.Id("ctl00_plhMain_cboPurpose"))).SelectByValue("1");
                Thread.Sleep(TimeSpan.FromSeconds(3));

                driver.SwitchTo().DefaultContent();
                driver.SwitchTo().Frame(0);
                driver.FindElementById("ctl00_plhMain_btnSubmit").Click();

                driver.SwitchTo().DefaultContent();
                driver.SwitchTo().Frame(0);
                new SelectElement(new WebDriverWait(driver, TimeSpan.FromMinutes(10)).Until(drv => drv.FindElement(By.Id("ctl00_plhMain_cboVisaCategory")))).SelectByValue("229");
                Thread.Sleep(TimeSpan.FromSeconds(3));

                driver.SwitchTo().DefaultContent();
                driver.SwitchTo().Frame(0);
                driver.FindElementById("ctl00_plhMain_btnSubmit").Click();

                Thread.Sleep(TimeSpan.FromSeconds(5));
                driver.SwitchTo().DefaultContent();
                driver.SwitchTo().Frame(0);
                try
                {
                    var elem = driver.FindElementById("ctl00_plhMain_lblMsg");
                    if (!elem.Text.Contains("No date(s)"))
                    {
                        throw new NoSuchElementException();
                    }
                }
                catch (NoSuchElementException)
                {
                    break;
                }
            } while (true);

            player.Play();
            MessageBox.Show("Gotcha", "Success");

            main.TopMost = true;
            main.Focus();
            main.BringToFront();
            main.Activate();
        }
    }
}
