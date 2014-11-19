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
  public partial class Header : TBase
  {
    private int _orderId;
    private string _sk;

    public int OrderId
    {
      get
      {
        return _orderId;
      }
      set
      {
        __isset.orderId = true;
        this._orderId = value;
      }
    }

    public string Sk
    {
      get
      {
        return _sk;
      }
      set
      {
        __isset.sk = true;
        this._sk = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool orderId;
      public bool sk;
    }

    public Header() {
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
            if (field.Type == TType.I32) {
              OrderId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Sk = iprot.ReadString();
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
      TStruct struc = new TStruct("Header");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.orderId) {
        field.Name = "orderId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(OrderId);
        oprot.WriteFieldEnd();
      }
      if (Sk != null && __isset.sk) {
        field.Name = "sk";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Sk);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("Header(");
      sb.Append("OrderId: ");
      sb.Append(OrderId);
      sb.Append(",Sk: ");
      sb.Append(Sk);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
