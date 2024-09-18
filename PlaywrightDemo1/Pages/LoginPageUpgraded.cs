using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightDemo1.Pages
{
    public class LoginPageUpgraded
    {
        private IPage _page;
        public LoginPageUpgraded(IPage page) => _page = page; // This is the constructor

        private ILocator _lnkLogin => _page.Locator("text=Login");
        private ILocator _txtUsername => _page.Locator("#UserName");
        private ILocator _txtPassword => _page.Locator("#Password");
        private ILocator _btnLogin => _page.Locator("Log in");
        private ILocator _lnkEmployeeLists => _page.Locator("Employee Details");

        [Obsolete]
        public async Task ClickLogin()
        {
            await _page.RunAndWaitForNavigationAsync(async () =>
            {
                await _lnkLogin.ClickAsync();
            }, new PageRunAndWaitForNavigationOptions
            {
                UrlString = "**/Login"
            });
        }

        public async Task ClickEmployeeList() => await _lnkEmployeeLists.ClickAsync();

        public async Task Login(string userName, string password)
        {
            await _txtUsername.FillAsync(userName);
            await _txtPassword.FillAsync(password);
            await _btnLogin.ClickAsync();
        }
        public async Task<bool> IsEmployeeDetailsExists() => await _lnkEmployeeLists.IsVisibleAsync();
    }
}
