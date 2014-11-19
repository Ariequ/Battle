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

namespace Hero.Config
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class FuncGroup : TBase
  {
    private int _id;
    private List<FuncData> _funcDataList;

    public int Id
    {
      get
      {
        return _id;
      }
      set
      {
        __isset.id = true;
        this._id = value;
      }
    }

    public List<FuncData> FuncDataList
    {
      get
      {
        return _funcDataList;
      }
      set
      {
        __isset.funcDataList = true;
        this._funcDataList = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool id;
      public bool funcDataList;
    }

    public FuncGroup() {
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
              Id = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                FuncDataList = new List<FuncData>();
                TList _list58 = iprot.ReadListBegin();
                for( int _i59 = 0; _i59 < _list58.Count; ++_i59)
                {
                  FuncData _elem60 = new FuncData();
                  _elem60 = new FuncData();
                  _elem60.Read(iprot);
                  FuncDataList.Add(_elem60);
                }
                iprot.ReadListEnd();
              }
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
      TStruct struc = new TStruct("FuncGroup");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.id) {
        field.Name = "id";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Id);
        oprot.WriteFieldEnd();
      }
      if (FuncDataList != null && __isset.funcDataList) {
        field.Name = "funcDataList";
        field.Type = TType.List;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, FuncDataList.Count));
          foreach (FuncData _iter61 in FuncDataList)
          {
            _iter61.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("FuncGroup(");
      sb.Append("Id: ");
      sb.Append(Id);
      sb.Append(",FuncDataList: ");
      sb.Append(FuncDataList);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
