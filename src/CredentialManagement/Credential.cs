﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace CredentialManagement
{
    public class Credential: IDisposable
    {

        static object _lockObject = new object();
        bool _disposed;

        static SecurityPermission _unmanagedCodePermission;

        CredentialType _type;
        StringFormat _format;
        string _target;
        SecureString _password;
        string _username;
        string _description;
        DateTime _lastWriteTime;
        PersistanceType _persistanceType;

        static Credential()
        {
            lock (_lockObject)
            {
                _unmanagedCodePermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
            }
        }
        public Credential()
            : this(null)
        {
        }

        public Credential(string username)
            : this(username, null)
        {
        }

        public Credential(string username, string password)
            : this(username, password, null)
        {
        }

        public Credential(string username, string password, string target)
            : this(username, password, target, CredentialType.Generic)
        {
        }

        public Credential(string username, string password, string target, CredentialType type)
            : this(username, password, target, type, StringFormat.Unicode)
        {
        }

        public Credential(string username, string password, string target, CredentialType type, StringFormat format)
        {
            Username = username;
            Password = password;
            Target = target;
            Type = type;
            PersistanceType = PersistanceType.Session;
            _lastWriteTime = DateTime.MinValue;
        }


        public void Dispose()
        {
            Dispose(true);

            // Prevent GC Collection since we have already disposed of this object
            GC.SuppressFinalize(this);
        }
        ~Credential()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    SecurePassword.Clear();
                    SecurePassword.Dispose();
                }
            }
            _disposed = true;
        }

        private void CheckNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Credential object is already disposed.");
            }
        }


        public string Username {
            get
            {
                CheckNotDisposed();
                return _username;
            }
            set
            {
                CheckNotDisposed();
                _username = value;
            }
        }
        public string Password
        {
            get
            {
                return SecureStringHelper.CreateString(SecurePassword);
            }
            set
            {
                CheckNotDisposed();
                SecurePassword = SecureStringHelper.CreateSecureString(string.IsNullOrEmpty(value) ? string.Empty : value);
            }
        }
        public SecureString SecurePassword
        {
            get
            {
                CheckNotDisposed();
                _unmanagedCodePermission.Demand();
                return null == _password ? new SecureString() : _password.Copy();
            }
            set
            {
                CheckNotDisposed();
                if (null != _password)
                {
                    _password.Clear();
                    _password.Dispose();
                }
                _password = null == value ? new SecureString() : value.Copy();
            }
        }
        public string Target
        {
            get
            {
                CheckNotDisposed();
                return _target;
            }
            set
            {
                CheckNotDisposed();
                _target = value;
            }
        }

        public string Description
        {
            get
            {
                CheckNotDisposed();
                return _description;
            }
            set
            {
                CheckNotDisposed();
                _description = value;
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                return LastWriteTimeUtc.ToLocalTime();
            }
        }
        public DateTime LastWriteTimeUtc 
        { 
            get
            {
                CheckNotDisposed();
                return _lastWriteTime;
            }
            private set { _lastWriteTime = value; }
        }

        public CredentialType Type
        {
            get
            {
                CheckNotDisposed();
                return _type;
            }
            set
            {
                CheckNotDisposed();
                _type = value;
            }
        }

        public StringFormat Format 
        {
            get
            {
                CheckNotDisposed();
                return _format;
            }
            set
            {
                CheckNotDisposed();
                _format = value;
            }
        }


        public PersistanceType PersistanceType
        {
            get
            {
                CheckNotDisposed();
                return _persistanceType;
            }
            set
            {
                CheckNotDisposed();
                _persistanceType = value;
            }
        }

        public bool Save()
        {
            CheckNotDisposed();
            _unmanagedCodePermission.Demand();

            var FormatProvider = StringFormatProvider.GetProvider(Format);
            byte[] passwordBytes = FormatProvider.GetBytes(Password);
            if (Password.Length > (512))
            {
                throw new ArgumentOutOfRangeException("The password has exceeded 512 bytes.");
            }

            NativeMethods.CREDENTIAL credential = new NativeMethods.CREDENTIAL();
            credential.TargetName = Target;
            credential.UserName = Username;
            credential.CredentialBlob = FormatProvider.StringToCoTaskMem(Password);
            credential.CredentialBlobSize = passwordBytes.Length;
            credential.Comment = Description;
            credential.Type = (int)Type;
            credential.Persist = (int) PersistanceType;

            bool result = NativeMethods.CredWrite(ref credential, 0);
            if (!result)
            {
                return false;
            }
            LastWriteTimeUtc = DateTime.UtcNow;
            return true;
        }

        public bool Delete()
        {
            CheckNotDisposed();
            _unmanagedCodePermission.Demand();

            if (string.IsNullOrEmpty(Target))
            {
                throw new InvalidOperationException("Target must be specified to delete a credential.");
            }

            StringBuilder target = string.IsNullOrEmpty(Target) ? new StringBuilder() : new StringBuilder(Target);
            bool result = NativeMethods.CredDelete(target, Type, 0);
            return result;
        }

        public bool Load()
        {
            CheckNotDisposed();
            _unmanagedCodePermission.Demand();

            IntPtr credPointer;

            bool result = NativeMethods.CredRead(Target, Type, 0, out credPointer);
            if (!result)
            {
                return false;
            }
            using (NativeMethods.CriticalCredentialHandle credentialHandle = new NativeMethods.CriticalCredentialHandle(credPointer))
            {
                LoadInternal(credentialHandle.GetCredential());
            }
            return true;
        }

        public bool Exists()
        {
            CheckNotDisposed();
            _unmanagedCodePermission.Demand();

            if (string.IsNullOrEmpty(Target))
            {
                throw new InvalidOperationException("Target must be specified to check existance of a credential.");
            }

            using (Credential existing = new Credential { Target = Target, Type = Type })
            {
                return existing.Load();
            }
        }

        internal void LoadInternal(NativeMethods.CREDENTIAL credential)
        {
            Username = credential.UserName;
            if (credential.CredentialBlobSize > 0)
            {
                Password = StringFormatProvider.GetProvider(Format).PtrToString(credential.CredentialBlob, credential.CredentialBlobSize);
            }
            Target = credential.TargetName;
            Type = (CredentialType)credential.Type;
            PersistanceType = (PersistanceType)credential.Persist;
            Description = credential.Comment;
            LastWriteTimeUtc = DateTime.FromFileTimeUtc(credential.LastWritten);
        }
    }
}