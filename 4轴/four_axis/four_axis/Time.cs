using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace four_axis
{
    class Time
    {
        private int hour;
        private int minute;
        private int second;

        //构造函数，TIME类将首先执行该函数
        public Time()
        {
            this.hour = 0;
            this.minute = 0;
            this.second = 0;
        }
        //重载构造函数
        public Time(int hour, int minute, int second)
        {
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }
        //从外部将始终参数录入
        public void sethour(int hour)
        {
            this.hour = hour;
        }
        public void setminute(int minute)
        {
            this.minute = minute;
        }
        public void setsecond(int second)
        {
            this.second = second;
        }
        //向外部返回信息
        public int Gethour()
        {
            return this.hour;
        }
        public int Getminute()
        {
            return this.minute;
        }
        public int Getsecond()
        {
            return this.second;
        }

        //对系统++运算符进行重新定义；在实例化一个类的时候将由于++运算而触发；
        public static Time operator ++(Time time)
        {
            time.second++;
            if (time.second >= 60)
            {
                time.minute++;
                time.second = 0;
                if (time.minute >= 60)
                {
                    time.hour++;
                    time.minute = 0;
                    time.second = 0;
                    if (time.hour >= 24)
                    {
                        time.minute = 0;
                        time.second = 0;
                        time.hour = 0;
                    }
                }
            }
            return new Time(time.hour, time.minute, time.second);
        }
    }
}
