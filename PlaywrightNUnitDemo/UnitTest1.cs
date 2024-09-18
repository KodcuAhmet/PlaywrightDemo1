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
        public async Task Test1()
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await Page.GetByLabel("UserName").DblClickAsync();
            await Page.GetByLabel("UserName").ClickAsync();
            await Page.GetByLabel("UserName").FillAsync("admin");
            await Page.GetByLabel("UserName").PressAsync("Tab");
            await Page.GetByLabel("Password").FillAsync("password");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Employee List" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Create New" }).ClickAsync();
            await Page.GetByRole(AriaRole.Heading, new() { Name = "Employee" }).ClickAsync();
            await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Employee" })).ToBeVisibleAsync();

        }
    }
}