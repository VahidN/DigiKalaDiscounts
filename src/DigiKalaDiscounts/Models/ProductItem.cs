using DNTPersianUtils.Core;

namespace DigiKalaDiscounts.Models;

public class ProductItem
{
    public required string Id { set; get; }

    public DateTime AddDate { set; get; }

    public long Timer { set; get; }

    public DateTime EndDate => AddDate.AddSeconds(Timer);

    public int Retries { set; get; }

    public bool IsDone { set; get; }

    public string Error { set; get; } = string.Empty;

    public string? Title { set; get; }

    public string? Url { set; get; }

    public string? ImageUrl { set; get; }

    public long SellingPrice { set; get; }

    public long RrpPrice { set; get; }

    public long SoldPercentage { set; get; }

    public long Discount => RrpPrice - SellingPrice;

    public long DiscountPercent { set; get; }

    /// <summary>
    ///     https://core.telegram.org/bots/api#html-style
    /// </summary>
    public override string ToString() =>
        $"""
         <a href="https://www.digikala.com{Url}"><b>{Title.ToPersianNumbers().Trim()}</b></a>
         """ +
        $"""

             <b>قيمت اصلى (تومان):</b> {RrpPrice:n0}
             <b>درصد تخفيف:</b> {DiscountPercent}%
             <b>ميزان تخفيف (تومان):</b> {Discount:n0}
             <b>قيمت فروش پس از تخفيف (تومان):</b> {SellingPrice:n0}
             <b>زمان پايان تخفيف:</b> {EndDate.ToFriendlyPersianDateTextify()}
             {(SoldPercentage > 0 ? $"<b>فروش رفته:</b> {SoldPercentage}%" : "")}
             """
            .ToPersianNumbers();
}
