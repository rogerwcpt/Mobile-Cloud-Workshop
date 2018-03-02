using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ContosoFieldService.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin Forms!"));
            app.Screenshot("Welcome screen.");

            Assert.IsTrue(results.Any());
        }

        [Test]
        public void ViewJobDetails()
        {
            //Arrange

            string strFullName = "Mike James";
            string strEmail = "mike@xamarin.com";

            //Act

            /* Wait for a login element to appear on the screen.
             * This will also synchronize screenshots.
             * Using the Marked query targeting the element AutomationID so the
             * test will mostly work cross platform.
            */
            app.WaitForElement(x => x.Marked("tbxFullname"));

            // Taking more screenshots than normal for demo purposes
            app.Screenshot("App Launched");

            // Enter Username
            app.EnterText(x => x.Marked("tbxFullname"), strFullName);
            app.Screenshot("Full Name entered");

            // Enter Email
            app.EnterText(x => x.Marked("tbxEmail"), strEmail);
            app.Screenshot("Email Entered");

            // Tap the Login button
            app.Tap(x => x.Marked("btnLogin"));

            /* Wait for the jobs listo appear.
            * If no jobs are in the database, the test will throw a timeout exception
            */
            AppResult[] jobs = app.WaitForElement(x => x.Marked("lblName"));
            app.Screenshot("List of available jobs");

            // Tap the first job on the list
            app.Tap(x => x.Marked("lblName").Index(0));
            app.WaitForElement(x => x.Marked("btnStartJob"));
            app.Screenshot("Job Details");

            // Assert

            // Query the job details page and make sure the title is not empty
            AppResult[] results = app.Query(x => x.Marked("lblDetailsTitle"));
            Assert.IsNotEmpty(results[0].Text);
        }
    }
}
