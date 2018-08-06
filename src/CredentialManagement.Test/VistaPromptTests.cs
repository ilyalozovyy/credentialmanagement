using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CredentialManagement.Test
{
    [TestClass]
    public class VistaPromptTests
    {
        static string MAX_LENGTH_VALIDATION_TEXT;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 50000; i++)
            {
                sb.Append('A');
            }
            MAX_LENGTH_VALIDATION_TEXT = sb.ToString();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            MAX_LENGTH_VALIDATION_TEXT = null;
        }


        [TestMethod]
        public void VistaPrompt_Create_ShouldNotBeNull()
        {
            new VistaPrompt().ShouldNotBeNull();
        }

        [TestMethod]
        public void VistaPrompt_ShouldImplement_IDisposable()
        {
            Assert.IsTrue(new VistaPrompt() is IDisposable, "VistaPrompt should implement IDisposable interface.");
        }

        [TestMethod]
        public void VistaPrompt_Username_MaxLength()
        {
            Testing.ShouldThrowException<ArgumentOutOfRangeException>(
                () => new VistaPrompt { Username = MAX_LENGTH_VALIDATION_TEXT });
        }

        [TestMethod]
        public void VistaPrompt_Username_NullValue()
        {
            Testing.ShouldThrowException<ArgumentNullException>(() => new VistaPrompt { Username = null });
        }

        [TestMethod]
        public void VistaPrompt_Password_NullValue()
        {
            Testing.ShouldThrowException<ArgumentNullException>(() => new VistaPrompt { Password = null });
        }

        [TestMethod]
        public void VistaPrompt_Message_MaxLength()
        {
            Testing.ShouldThrowException<ArgumentOutOfRangeException>(
                () => new VistaPrompt { Message = MAX_LENGTH_VALIDATION_TEXT });
        }

        [TestMethod]
        public void VistaPrompt_Message_NullValue()
        {
            Testing.ShouldThrowException<ArgumentNullException>(() => new VistaPrompt { Message = null });
        }

        [TestMethod]
        public void VistaPrompt_Title_MaxLength()
        {
            Testing.ShouldThrowException<ArgumentOutOfRangeException>(
                () => new VistaPrompt { Title = MAX_LENGTH_VALIDATION_TEXT });
        }

        [TestMethod]
        public void VistaPrompt_Title_NullValue()
        {
            Testing.ShouldThrowException<ArgumentNullException>(() => new VistaPrompt { Title = null });
        }

        [TestMethod]
        public void VistaPrompt_ShowDialog_ShouldNotThrowError()
        {
            VistaPrompt prompt = GetDefaultPrompt();
            prompt.ShowDialog();
            prompt.Dispose();
        }

        [TestMethod]
        public void VistaPrompt_ShowDialog_WithParent_ShouldNotThrowError()
        {
            VistaPrompt prompt = GetDefaultPrompt();
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }


        [TestMethod]
        public void VistaPrompt_ShowDialog_With_Username()
        {
            VistaPrompt prompt = GetDefaultPrompt();
            prompt.Username = "username";
            prompt.ShowDialog().ShouldEqual(DialogResult.OK);
            prompt.Dispose();
        }

        [TestMethod]
        public void VistaPrompt_ShowDialog_GenericCredentials()
        {
            VistaPrompt prompt = GetDefaultPrompt();
            prompt.Title = "Please provide credentials";
            prompt.GenericCredentials = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void VistaPrompt_ShowDialog_ShowSaveCheckBox()
        {
            VistaPrompt prompt = GetDefaultPrompt();
            prompt.ShowSaveCheckBox = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        private VistaPrompt GetDefaultPrompt()
        {
            return new VistaPrompt();
        }
    }
}
