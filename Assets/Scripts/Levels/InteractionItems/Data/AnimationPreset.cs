namespace MoroshkovieKochki
{
    public struct AnimationPreset
    {
        public string Idle;
        public string Walk;
        public string Hello;
        public string Take;
        public string Say;
        public string Think;
        public string Hit;

        public AnimationPreset(string idle, string walk, string hello, string take, string say, string think, string hit)
        {
            Idle = idle;
            Walk = walk;
            Hello = hello;
            Take = take;
            Say = say;
            Think = think;
            Hit = hit;
        }
    }
}