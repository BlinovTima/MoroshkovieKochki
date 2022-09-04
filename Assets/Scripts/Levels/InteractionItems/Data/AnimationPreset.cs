namespace MoroshkovieKochki
{
    public struct AnimationPreset
    {
        public string Idle;
        public string Walk;
        public string Hello;
        public string Take;
        public string Say;
        public string Hit;
        public string No;
        public string ThinkStart;
        public string ThinkLoop;
        public string ThinkFinish;

        public AnimationPreset(string idle, 
            string walk, 
            string hello,
            string take, 
            string say, 
            string thinkStart, 
            string hit, 
            string thinkLoop,
            string thinkFinish, 
            string no)
        {
            Idle = idle;
            Walk = walk;
            Hello = hello;
            Take = take;
            Say = say;
            ThinkStart = thinkStart;
            Hit = hit;
            ThinkLoop = thinkLoop;
            ThinkFinish = thinkFinish;
            No = no;
        }
    }
}