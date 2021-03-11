using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiaryApp.Test
{
  [TestClass]
  public class TestsInitialize
  {
    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
      Effort.Provider.EffortProviderConfiguration.RegisterProvider();
    }

    [TestInitialize]
    public void MyTestInitialize()
    {
      EffortProviderFactory.ResetDb();
    }
  }
}
