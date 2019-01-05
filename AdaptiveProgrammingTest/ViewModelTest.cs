using AdaptiveProgrammingModel;
using AdaptiveProgrammingViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdaptiveProgrammingTest
{
    [TestClass]
    public class ViewModelTest
    {
        private MainViewModel mainViewModel;

        [TestInitialize]
        public void ModelTestInitialize()
        {
            mainViewModel = new MainViewModel();
            mainViewModel.DLLFileBrowser = new TestBrowse();
        }

        [TestMethod]
        public void MainViewModelBrowseDLLFile()
        {
            mainViewModel.BrowseDLLFile();
            Assert.AreEqual(mainViewModel.DLLPath, "test.dll");
            Assert.AreEqual(mainViewModel.ChangeLoadButtonState, true);
        }

    }
}