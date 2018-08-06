using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CredentialManagement.Test
{
    [TestClass]
    public class XPPromptTests
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
        public void XPPrompt_Create_ShouldNotBeNull()
        {
            new XPPrompt().ShouldNotBeNull();
        }

        [TestMethod]
        public void XPPrompt_ShouldImplement_IDisposable()
        {
            Assert.IsTrue(new XPPrompt() is IDisposable, "XPPrompt should implement IDisposable interface.");
        }

        [TestMethod]
        public void XPPrompt_Username_MaxLength()
        {
            Testing.ShouldThrowException<ArgumentOutOfRangeException>(
                () => new XPPrompt {Username = MAX_LENGTH_VALIDATION_TEXT});
        }  
        
        [TestMethod]
        public void XPPrompt_Username_NullValue()
        {
            Testing.ShouldThrowException<ArgumentNullException>(() => new XPPrompt { Username = null });
        }

        [TestMethod]
        public void XPPrompt_Password_NullValue()
        {
            Testing.ShouldThrowException<ArgumentNullException>(() => new XPPrompt {Password = null});
        }

        [TestMethod]
        public void XPPrompt_Target_NullValue()
        {
            Testing.ShouldThrowException<ArgumentNullException>(() => new XPPrompt {Target = null});
        }

        [TestMethod]
        public void XPPrompt_Message_MaxLength()
        {
            Testing.ShouldThrowException<ArgumentOutOfRangeException>(
                () => new XPPrompt { Message = MAX_LENGTH_VALIDATION_TEXT });
        }

        [TestMethod]
        public void XPPrompt_Message_NullValue()
        {
            Testing.ShouldThrowException<ArgumentNullException>(() => new XPPrompt { Message = null });
        }

        [TestMethod]
        public void XPPrompt_Title_MaxLength()
        {
            Testing.ShouldThrowException<ArgumentOutOfRangeException>(
                () => new XPPrompt {Title = MAX_LENGTH_VALIDATION_TEXT});
        }

        [TestMethod]
        public void XPPrompt_Title_NullValue()
        {
            Testing.ShouldThrowException<ArgumentNullException>(() => new XPPrompt { Title = null });
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_ShouldNotThrowError()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.ShowDialog();
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_WithParent_ShouldNotThrowError()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_Without_Target_ShouldThrowError()
        {
            XPPrompt prompt = new XPPrompt();
            Testing.ShouldThrowException<InvalidOperationException>(() => prompt.ShowDialog());
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_With_Username()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.Username = "username";
            prompt.Title = "Enter enter valid credentials.";
            prompt.Message = "Please enter valid credentials.";
            prompt.ShowSaveCheckBox = true;
            prompt.GenericCredentials = true;
            prompt.ShowDialog().ShouldEqual(DialogResult.OK);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_AlwaysShowUI()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.AlwaysShowUI = true;
            prompt.GenericCredentials = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_AlwaysShowUI_Without_GenericCredentials_ShouldThrowError()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.AlwaysShowUI = true;
            Testing.ShouldThrowException<InvalidOperationException>(() => prompt.ShowDialog(IntPtr.Zero));
            prompt.Dispose();
        }
        
        [TestMethod]
        public void XPPrompt_ShowDialog_CompleteUsername()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.CompleteUsername = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_DoNotPersist()
        {
            XPPrompt prompt = GetDefaultPrompt();
                    prompt.DoNotPersist = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }
        
        [TestMethod]
        public void XPPrompt_ShowDialog_ExcludeCertificates()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.ExcludeCertificates = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_ExpectConfirmation()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.ExpectConfirmation = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_GenericCredentials()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.GenericCredentials = true;
            prompt.ShowDialog(IntPtr.Zero).ShouldEqual(DialogResult.OK);
            prompt.Dispose();
        }
        
        [TestMethod]
        public void XPPrompt_ShowDialog_IncorrectPassword()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.IncorrectPassword = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_Persist()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.Persist = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_RequestAdministrator()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.RequestAdministrator = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_RequreCertificate()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.RequireCertificate = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_RequreSmartCard()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.RequireSmartCard = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_ShowSaveCheckBox()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.ShowSaveCheckBox = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_UsernameReadOnly()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.UsernameReadOnly = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        [TestMethod]
        public void XPPrompt_ShowDialog_ValidateUsername()
        {
            XPPrompt prompt = GetDefaultPrompt();
            prompt.ValidateUsername = true;
            prompt.ShowDialog(IntPtr.Zero);
            prompt.Dispose();
        }

        private XPPrompt GetDefaultPrompt()
        {
            XPPrompt prompt = new XPPrompt {Target = "target"};
            return prompt;
        }
    }
}
