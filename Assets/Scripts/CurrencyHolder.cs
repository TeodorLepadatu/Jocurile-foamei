public class CurrencyHolder
{
    private static int currency = 0;

    public static void addCurrency(int amount) {
        currency += amount;
    }
    public static int getCurrency() {
        return currency;
    }
    public static void reset() {
        currency = 0;
    }
    public static void setCurrency(int amount) {
        currency = amount;
    }
}
