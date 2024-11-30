namespace PresenceBackend.Modules;

public class RateLimiter
{
    public TimeSpan Limit { get; private set; }
    public int Burst { get; private set; }
    public int Tokens { get; private set; }
    public DateTime Last { get; private set; }

    public RateLimiter(TimeSpan limit, int burst)
    {
        Limit = limit;
        Burst = burst;
        Tokens = burst;
    }

    private Reservation CalcReservation(bool success) =>
        new Reservation()
        {
            Success = success,
            Remaining = Tokens,
            Reset = Last.Add(Limit),
            Burst = Burst,
        };

    public Reservation Reserve(int n = 1)
    {
        if (n <= 0)
        {
            return CalcReservation(true);
        }

        if (Last != default(DateTime))
        {
            var tokensSinceLast = (int)Math.Floor(DateTime.Now.Subtract(Last) / Limit);
            Tokens += tokensSinceLast;
        }

        if (Tokens > Burst)
        {
            Tokens = Burst;
        }

        if (Tokens >= n)
        {
            Tokens -= n;
            Last = DateTime.Now;

            return CalcReservation(true);
        }

        return CalcReservation(false);
    }
    
    public bool Allow(int n = 1) =>
        Reserve(n).Success;
}