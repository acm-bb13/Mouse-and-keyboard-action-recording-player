﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 动作监听播放器
{
    //用来储存动作信息
    public class Date1
    {
        //记录xx时刻，鼠标还是键盘，哪个按键，按下还是松开
        public Int64 timeTickRecord;

        //枚举
        public enum IsMouseOrKeyboard { Mouse , Keyboard };
        public IsMouseOrKeyboard isMouseOrKeyboard;

        //是按下还是抬起
        public enum IsUpOrDown { Up, Down,Move,Wheel};
        public IsUpOrDown isUpOrDown;

        //如果是键盘，保存键盘事件信息
        public KeyEventArgs keyEventArgs;

        //如果是鼠标，保存鼠标事件信息
        public MouseEventArgs mouseEventArgs;


        //一个用来创建键盘，一个用来创建鼠标事件
        public Date1(long timeTickRecord, IsMouseOrKeyboard isMouseOrKeyboard, IsUpOrDown isUpOrDown, MouseEventArgs mouseEventArgs)
        {
            this.timeTickRecord = timeTickRecord;
            this.isMouseOrKeyboard = isMouseOrKeyboard;
            this.isUpOrDown = isUpOrDown;
            this.mouseEventArgs = mouseEventArgs;
        }

        public Date1(long timeTickRecord, IsMouseOrKeyboard isMouseOrKeyboard, IsUpOrDown isUpOrDown, KeyEventArgs keyEventArgs)
        {
            this.timeTickRecord = timeTickRecord;
            this.isMouseOrKeyboard = isMouseOrKeyboard;
            this.isUpOrDown = isUpOrDown;
            this.keyEventArgs = keyEventArgs;
        }

        public static Date1 create(long timeTickRecord, IsMouseOrKeyboard isMouseOrKeyboard, IsUpOrDown isUpOrDown, EventArgs eventArgs)
        {
            Date1 date1 = null;
            if (isMouseOrKeyboard == IsMouseOrKeyboard.Mouse)
            {
                date1 = new Date1(timeTickRecord, isMouseOrKeyboard, isUpOrDown, (MouseEventArgs)eventArgs);
            }
            if (isMouseOrKeyboard == IsMouseOrKeyboard.Keyboard)
            {
                date1 = new Date1(timeTickRecord, isMouseOrKeyboard, isUpOrDown, (KeyEventArgs)eventArgs);
            }
            return date1;
        }

        public static Date1 create(
            long timeTickRecord, 
            IsMouseOrKeyboard isMouseOrKeyboard,
            IsUpOrDown isUpOrDown,
            KeyEventArgs keyEventArgs,
            MouseButtons mouseButtons , 
            int X ,
            int Y ,
            int MouseDelta)
        {
            Date1 date1 = null;
            if (isMouseOrKeyboard == IsMouseOrKeyboard.Mouse)
            {
                date1 = new Date1(timeTickRecord, isMouseOrKeyboard, isUpOrDown,new MouseEventArgs(mouseButtons , 0 ,X,Y,MouseDelta) );
            }
            if (isMouseOrKeyboard == IsMouseOrKeyboard.Keyboard)
            {
                date1 = new Date1(timeTickRecord, isMouseOrKeyboard, isUpOrDown,keyEventArgs);
            }
            return date1;
        }
    }
}
