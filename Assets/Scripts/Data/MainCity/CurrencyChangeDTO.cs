using System;

public struct CurrencyChangeDTO
{
    public CurrencyType type;

    public int newValue;

    public CurrencyChangeDTO(CurrencyType type, int newValue)
    {
        this.type = type;
        this.newValue = newValue;
    }
}

