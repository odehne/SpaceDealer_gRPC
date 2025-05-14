
using Terminal.Gui;

var win = new Window()
{
    Width = Dim.Fill(),
    Height = Dim.Fill()
};

Application.Init();

Application.Top.Add(win);
//var button = new Button
//{
//    Text = "Hello"
//};
//win.Add(button);


var frame1 = new FrameView("Frame 1")
{
    Width = Dim.Percent(50),
    Height = Dim.Fill(),
};

var frame2 = new FrameView("Frame 2")
{
    X = Pos.Right(frame1),
    Width = Dim.Percent(50),
    Height = Dim.Fill(),
};

var lbl1 = new Label("Frame contents 1")
{
    Height =1,
    Width = 20,
};

var lbl2= new Label("Frame contents 2")
{
    Height = 1,
    Width = 20,
};

frame1.Add(lbl1);
frame2.Add(lbl2);

win.Add(frame1, frame2);

Application.Run();