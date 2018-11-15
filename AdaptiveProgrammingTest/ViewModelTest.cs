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
        [TestMethod]
        public void MainViewModelLoadDLLFile()
        {
            mainViewModel.DLLPath = "../../../TestFile/TPA.ApplicationArchitecture.dll";
            mainViewModel.LoadDLLFile();
            Assert.AreNotEqual(mainViewModel.AssemblyView,null);
            Assert.AreEqual(mainViewModel.TreeViewArea[0],mainViewModel.AssemblyView);
            Assert.AreEqual(mainViewModel.ChangeLoadButtonState, false);
            Assert.AreEqual(mainViewModel.ChangeSerializeButtonState, true);
        }
    }
}