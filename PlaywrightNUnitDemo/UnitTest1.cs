using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Threading.Tasks;

namespace PlaywrightNUnitDemo
{
    public class Tests : PageTest
    {
        [SetUp]
        public async Task Setup()
        {
            await Page.GotoAsync("http://eaapp.somee.com/");
        }

        [Test]
        [TestCaseSource(nameof(Login))]
        public async Task Test1(LoginModel login)
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await Page.GetByLabel("UserName").DblClickAsync();
            await Page.GetByLabel("UserName").ClickAsync();
            await Page.GetByLabel("UserName").FillAsync(login.UserName);
            await Page.GetByLabel("Password").FillAsync(login.Password);
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Employee List" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Create New" }).ClickAsync();
            await Page.GetByRole(AriaRole.Heading, new() { Name = "Employee" }).ClickAsync();
            await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Employee" })).ToBeVisibleAsync();

        }

        public static IEnumerable<LoginModel> Login()
        {
            yield return new LoginModel()
            {
                UserName = "admin",
                Password = "password"
            };
        }
    }
}