public class CurrencyHolder
{
    private static int currency = 0;

    public static void addCurrency(int ammount) {
        currency += ammount;
    }
    public static int getCurrency() {
        return currency;
    }
}
