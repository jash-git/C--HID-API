#region parameter Define
HIDInterface hid = new HIDInterface();


struct connectStatusStruct
{
    public bool preStatus;
    public bool curStatus;
}

connectStatusStruct connectStatus = new connectStatusStruct();

//推送連接狀態信息
public delegate void isConnectedDelegate(bool isConnected);
public isConnectedDelegate isConnectedFunc;


//推送接收數據信息
public delegate void PushReceiveDataDele(byte[] datas);
public PushReceiveDataDele pushReceiveData;

#endregion

//第一步需要初始化，傳入vid、pid，並開啓自動連接
public void Initial()
{ 

    hid.StatusConnected = StatusConnected;
    hid.DataReceived = DataReceived;

    HIDInterface.HidDevice hidDevice = new HIDInterface.HidDevice();
    hidDevice.vID =0x04D8;
    hidDevice.pID = 0x003F;
    hidDevice.serial = "";
    hid.AutoConnect(hidDevice);

}

//不使用則關閉
public void Close()
{
    hid.StopAutoConnect();
}

//發送數據
public bool SendBytes(byte[] data)
{
            
    return hid.Send(data);
            
}

//接受到數據
public void DataReceived(object sender, byte[] e)
{
    if (pushReceiveData != null)
        pushReceiveData(e);
}

//狀態改變接收
public void StatusConnected(object sender, bool isConnect)
{
    connectStatus.curStatus = isConnect;
    if (connectStatus.curStatus == connectStatus.preStatus)  //connect
        return;
    connectStatus.preStatus = connectStatus.curStatus;

    if(connectStatus.curStatus)
    {
        isConnectedFunc(true);
        //ReportMessage(MessagesType.Message, "連接成功");
    }
    else //disconnect
    {
        isConnectedFunc(false);
        //ReportMessage(MessagesType.Error, "無法連接");
    }
}