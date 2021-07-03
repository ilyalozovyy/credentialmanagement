using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CredentialManagement.Test
{
    [TestClass]
    public class CredentialSetTests
    {
        [TestMethod]
        public void CredentialSet_Create()
        {
            new CredentialSet();
        }

        [TestMethod]
        public void CredentialSet_Create_WithTarget()
        {
            new CredentialSet("target");
        }

        [TestMethod]
        public void CredentialSet_ShouldBeIDisposable()
        {
            Assert.IsTrue(new CredentialSet() is IDisposable, "CredentialSet needs to implement IDisposable Interface.");
        }

        [TestMethod]
        public void CredentialSet_Load()
        {
            Credential credential = new Credential
            {
                Username = "username",
                Password = "password",
                Target = "target",
                Type = CredentialType.Generic
            };
            credential.Save();

            CredentialSet set = new CredentialSet();
            set.Load();

            credential.Delete();

            set.Dispose();
        }

        [TestMethod]
        public void CredentialSet_Load_ShouldReturn_Self()
        {
            CredentialSet set = new CredentialSet();
            set.Load();

            set.Dispose();
        }

        [TestMethod]
        public void CredentialSet_Load_With_TargetFilter()
        {
            Credential credential = new Credential
            {
                Username = "filteruser",
                Password = "filterpassword",
                Target = "filtertarget"
            };
            credential.Save();

            CredentialSet set = new CredentialSet("filtertarget");
            set.Load();
            set.Dispose();
        }
    }
}
