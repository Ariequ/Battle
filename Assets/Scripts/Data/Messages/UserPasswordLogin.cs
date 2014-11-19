/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace Hero.Message.Auto
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class UserPasswordLogin : TBase
  {
    private string _username;
    private string _password;

    public string Username
    {
      get
      {
        return _username;
      }
      set
      {
        __isset.username = true;
        this._username = value;
      }
    }

    public string Password
    {
      get
      {
        return _password;
      }
      set
      {
        __isset.password = true;
        this._password = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool username;
      public bool password;
    }

    public UserPasswordLogin() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.String) {
              Username = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Password = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("UserPasswordLogin");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Username != null && __isset.username) {
        field.Name = "username";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Username);
        oprot.WriteFieldEnd();
      }
      if (Password != null && __isset.password) {
        field.Name = "password";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Password);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("UserPasswordLogin(");
      sb.Append("Username: ");
      sb.Append(Username);
      sb.Append(",Password: ");
      sb.Append(Password);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
