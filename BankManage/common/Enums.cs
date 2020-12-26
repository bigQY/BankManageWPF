namespace BankManage
{
    /// <summary>
    /// 利率类别
    /// </summary>
    public enum RateType
    {
        定期1年,
        定期3年,
        定期5年,
        定期超期部分,
        定期提前支取,
        活期,
        零存整取1年,
        零存整取3年,
        零存整取5年,
        零存整取超期部分,
        零存整取违规,
        个人贷款,
        企业贷款,
        助学贷款,
        学生短期贷款
    }

    /// <summary>
    /// 存款类别
    /// </summary>
    public enum MoneyAccountType
    {
        活期存款,
        定期存款,
        零存整取
    }

    public enum DealType
    {
        //TODO
    }
}
