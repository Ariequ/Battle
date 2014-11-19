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
  public partial class SoldiersConfig : TBase
  {
    private Dictionary<int, SoldierConfig> _cardConfigMap;

    public Dictionary<int, SoldierConfig> CardConfigMap
    {
      get
      {
        return _cardConfigMap;
      }
      set
      {
        __isset.cardConfigMap = true;
        this._cardConfigMap = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool cardConfigMap;
    }

    public SoldiersConfig() {
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
            if (field.Type == TType.Map) {
              {
                CardConfigMap = new Dictionary<int, SoldierConfig>();
                TMap _map10 = iprot.ReadMapBegin();
                for( int _i11 = 0; _i11 < _map10.Count; ++_i11)
                {
                  int _key12;
                  SoldierConfig _val13;
                  _key12 = iprot.ReadI32();
                  _val13 = new SoldierConfig();
                  _val13.Read(iprot);
                  CardConfigMap[_key12] = _val13;
                }
                iprot.ReadMapEnd();
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
      TStruct struc = new TStruct("SoldiersConfig");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (CardConfigMap != null && __isset.cardConfigMap) {
        field.Name = "cardConfigMap";
        field.Type = TType.Map;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteMapBegin(new TMap(TType.I32, TType.Struct, CardConfigMap.Count));
          foreach (int _iter14 in CardConfigMap.Keys)
          {
            oprot.WriteI32(_iter14);
            CardConfigMap[_iter14].Write(oprot);
          }
          oprot.WriteMapEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("SoldiersConfig(");
      sb.Append("CardConfigMap: ");
      sb.Append(CardConfigMap);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
