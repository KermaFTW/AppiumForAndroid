using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppiumForAndroid
{
	[TestClass]
	public class AndoridUITests
	{
		static TestContext _ctx;
		static private AppiumLocalService _appiumLocalService;

		[ClassInitialize]
		static public void Initialize(TestContext context)
		{
			_ctx = context;
		}

		[ClassCleanup]
		static public void CleanUp()
		{
			_appiumLocalService?.Dispose();
			_appiumLocalService = null;
		}

		[TestMethod]
		public void TestListInstalledPackages()
		{
			AndroidDriver<AppiumWebElement> driver = StartApp();

			string script = "mobile: shell";
			var arguments = new Dictionary<string, string>
			{
				{ "command", "pm list packages" },
				{ "--show-versioncode", "" }
			};

			var list = driver.ExecuteScript(script, arguments);
			Assert.IsNotNull(list);
			Console.WriteLine(list);
		}

		[TestMethod]
		public void AddOneSet()
		{
			AndroidDriver<AppiumWebElement> driver = StartApp();

			var el1 = driver.FindElement(MobileBy.Id("button_toggle_volume"));
			el1.Click();

			var setNumber = driver.FindElement(MobileBy.Id("edit_number_sets"));
			var numberOfSets = setNumber.Text;
			var expectedSetNumber = "3";
			Assert.IsTrue(numberOfSets == expectedSetNumber);
		}

		private AndroidDriver<AppiumWebElement> StartApp()
		{
			System.Environment.SetEnvironmentVariable("ANDROID_HOME", @"C:\Users\marko\AppData\Local\Android\Sdk");
			System.Environment.SetEnvironmentVariable("JAVA_HOME", @"C:\Program Files\Java\jdk1.8.0_161\");

			var cappabilities = new AppiumOptions();

			cappabilities.AddAdditionalCapability(MobileCapabilityType.DeviceName, "52003b08eae92517");
			cappabilities.AddAdditionalCapability(MobileCapabilityType.AutomationName, "UiAutomator2");

			var currentPath = Directory.GetCurrentDirectory();
			Console.WriteLine($"Current path: {currentPath}");
			var packagePath = Path.Combine(currentPath, @"..\..\..\TestApp\Interval Timer_v2.2.2_apkpure.com.apk");
			packagePath = Path.GetFullPath(packagePath);
			Console.WriteLine($"package path: {packagePath}");
			cappabilities.AddAdditionalCapability(MobileCapabilityType.App, packagePath);

			cappabilities.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, "cc.dreamspark.intervaltimer");
			cappabilities.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, ".MainActivity");

			var serveroptions = new OptionCollector();
			var relaxeSecurityOption = new KeyValuePair<string, string>("--relaxed-security", "");

			serveroptions.AddArguments(relaxeSecurityOption);
			var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().WithArguments(serveroptions).Build();
			_appiumLocalService.Start();
			var driver = new AndroidDriver<AppiumWebElement>(_appiumLocalService, cappabilities);

			return driver;
		}
	}
}
