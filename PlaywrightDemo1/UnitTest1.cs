using FluentAssertions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Constraints;
using PlaywrightDemo1.Pages;
using System.Web;

namespace PlaywrightDemo1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            //Playwright
            using var playwright = await Playwright.CreateAsync();
            //Browser
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            //Page
            var page = await browser.NewPageAsync();
            await page.GotoAsync("http://www.eaapp.somee.com");
            await page.ClickAsync("text=Login");
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = "EAApp.jpg"
            });
            await page.FillAsync("#UserName", "admin");
            await page.FillAsync("#Password", "password");
            await page.ClickAsync("text=Log in");
            var isExist = await page.Locator("text='Employee Details'").IsVisibleAsync();
            Assert.IsTrue(isExist);
        }

        [Test]
        public async Task TestWithPOM()
        {
            //Playwright
            using var playwright = await Playwright.CreateAsync();
            //Browser
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            //Page
            var page = await browser.NewPageAsync();
            await page.GotoAsync("http://www.eaapp.somee.com");

            var loginPage = new LoginPageUpgraded(page);
            await loginPage.ClickLogin();
            await loginPage.Login("admin", "password");
            var isExist = await loginPage.IsEmployeeDetailsExists();
            Assert.IsTrue(isExist);
        }


        [Test]
        public async Task WaitTest()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://demos.telerik.com/kendo-ui/window/angular");
            await page.Locator(
                    "text=Calendar May 2022SuMoTuWeThFrSa1234567891011121314151617181920212223242526272829 >> [aria-label=\"Close\"]")
                .ClickAsync();
            await page.Locator(
                    "text=Calendar May 2022SuMoTuWeThFrSa1234567891011121314151617181920212223242526272829 >> [aria-label=\"Close\"]")
                .ClickAsync();
            // Click button:has-text("Open AJAX content")
            await page.Locator("button:has-text(\"Open AJAX content\")").ClickAsync();
        }

        [Test]
        [Obsolete]
        public async Task TestNetwork()
        {
            //Playwright
            using var playwright = await Playwright.CreateAsync();
            //Browser
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            //Page
            var page = await browser.NewPageAsync();
            await page.GotoAsync("http://www.eaapp.somee.com");

            var loginPage = new LoginPageUpgraded(page);
            await loginPage.ClickLogin();
            await loginPage.Login("admin", "password");

            //var waitResponse = page.WaitForRequestAsync("**/Employee");
            //await loginPage.ClickEmployeeList();
            //var getResponse = await waitResponse;

            var response = await page.RunAndWaitForResponseAsync(async () =>
            {
                await loginPage.ClickEmployeeList();
            }, x => x.Url.Contains("/Employee") && x.Status == 200);

            var isExist = await loginPage.IsEmployeeDetailsExists();
            Assert.IsTrue(isExist);
        }

        [Test]
        public async Task Flipkart()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://flipkart.com/", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });
            await page.Locator("text=x").CheckAsync();

            await page.Locator("a", new PageLocatorOptions
            {
                HasTextString = "Login"
            }).ClickAsync();

            var request = await page.RunAndWaitForRequestAsync(async () =>
            {
                await page.Locator("text=x").CheckAsync();

            }, x => x.Url.Contains("flipkart.d1.sc.omtrdc.net") && x.Method == "GET");

            var returnData = HttpUtility.UrlDecode(request.Url);
            returnData.Should().Contain("Account Login:Displayed Exit");
        }

        [Test]
        public async Task? FlipkartNetworkInterception()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync();
            var page = await browser.NewPageAsync();

            page.Request += (_, request) => Console.WriteLine(request.Method + "---" + request.Url);
            page.Response += (_, response) => Console.WriteLine(response.Status + "---" + response.Url);

            //await page.RouteAsync("**/*", async route =>
            //{
            //    if (route.Request.ResourceType == "image")
            //        await route.AbortAsync();
            //    else
            //        await route.ContinueAsync();
            //});

            await page.GotoAsync("https://flipkart.com/", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });
        }
    }
}