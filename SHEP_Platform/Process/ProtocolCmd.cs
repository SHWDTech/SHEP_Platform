using System;
using Web.Common;

namespace SHEP_Platform.Process
{
    
    public class ProtocolCmd
    {

        public ProtocolCmd()
        {
            _nodeId = 0;
            _cmdType = 0;
            _cmdByte = 0;
            _srcAddr= 0;
            _dstAddr = 0;
            _datalen = 0;
        }

        #region Model
        private UInt32 _nodeId;
        private Byte _cmdType;
        private Byte _cmdByte;
        private byte[] _pwd = new byte[8];
        private byte[] _description = new byte [12];
        private byte _srcAddr;
        private byte _dstAddr;
        private byte[] _data = new byte[512];
        private UInt16 _datalen;
        /// <summary>
        /// 节点Id
        /// </summary>
        public UInt32 NodeId
        {
            set { _nodeId = value; }
            get { return _nodeId; }
        }
        /// <summary>
        /// 命令码
        /// </summary>
        public Byte CmdType
        {
            set { _cmdType = value; }
            get { return _cmdType; }
        }
        /// <summary>
        /// 命令码
        /// </summary>
        public Byte CmdByte
        {
            set { _cmdByte = value; }
            get { return _cmdByte; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public byte[] Pwd
        {
            set { _pwd = value; }
            get { return _pwd; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public byte[] Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 源地址
        /// </summary>
        public Byte SrcAddr
        {
            set { _srcAddr = value; }
            get { return _srcAddr; }
        }
        /// <summary>
        /// 目标地址
        /// </summary>
        public Byte DstAddr
        {
            set { _dstAddr = value; }
            get { return _dstAddr; }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        public byte[] Data
        {
            set { _data = value; }
            get { return _data; }
        }
        /// <summary>
        /// 发送数据长度
        /// </summary>
        public UInt16 DataLen
        {
            set { _datalen = value; }
            get { return _datalen; }
        }
        public UInt16 PacketLen
        {
            get { return (UInt16)(_datalen + 34); }
        }
        #endregion
        
        #region 命令
        public void EncodeFrame(ref byte[] buffer, ref int bufferLen)
        {
            UInt16 bufferIndex = 0;
            UInt16 crc = 0;
            
            buffer[bufferIndex] = 0xAA;
            bufferIndex += 1;

            buffer[bufferIndex] = this.CmdType;
            bufferIndex += 1;

            buffer[bufferIndex] = this.CmdByte;
            bufferIndex += 1;
            
            Array.Copy(this.Pwd, 0, buffer, bufferIndex, 8);
            bufferIndex += 8;

            Utility.UINT32ToBytes(this.NodeId, buffer, bufferIndex, false);
            bufferIndex += 4;

            Array.Copy(this.Description, 0, buffer, bufferIndex, 12);
            bufferIndex += 12;
            
            this.SrcAddr = 0x00;
            
            buffer[bufferIndex] = this.SrcAddr;
            bufferIndex += 1;

            this.DstAddr = 0x01;
            buffer[bufferIndex] = this.DstAddr;
            bufferIndex += 1;

            Utility.UINT16ToBytes(this.DataLen, buffer, bufferIndex, false);
            bufferIndex += 2;

            Array.Copy(this.Data, 0, buffer, bufferIndex, this.DataLen);
            bufferIndex += DataLen;

            //CRC 
            crc = CRC.GetUSMBCRC16(buffer, (UInt16)bufferIndex);

            Utility.UINT16ToBytes(crc, buffer, bufferIndex, false);
            //Array.Copy(buffer, bufferIndex, Utility.UINT16ToBytes(crc, false), 0, 2);
            bufferIndex += 2;

            buffer[bufferIndex] = 0xDD;
            bufferIndex += 1;

            bufferLen = bufferIndex;
            
            return;
        }

        public bool DecodeFrame(byte[] buffer, int bufferLen)
        {
            UInt16 bufferIndex = 0;
            UInt16 crc1 = 0;
            UInt16 crc2 = 0;
            
            if (buffer[bufferIndex] != 0xAA)
            {
                return false;
            }
            bufferIndex += 1;

            this.CmdType = buffer[bufferIndex];
            bufferIndex += 1;

            this.CmdByte = buffer[bufferIndex];
            bufferIndex += 1;

            Array.Copy(buffer, bufferIndex, this.Pwd, 0, 8);
            bufferIndex += 8;

            this.NodeId = Utility.BytesToUINT32(buffer, bufferIndex, false);
            bufferIndex += 4;

            Array.Copy(buffer, bufferIndex, this.Description, 0, 12);
            bufferIndex += 12;

            this.SrcAddr = buffer[bufferIndex];
            bufferIndex += 1;

            this.DstAddr = buffer[bufferIndex];
            bufferIndex += 1;

            this.DataLen = Utility.BytesToUINT16(buffer, bufferIndex, false);
            bufferIndex += 2;
            
            Array.Copy(buffer, bufferIndex, this.Data, 0, this.DataLen);
            bufferIndex += this.DataLen;
            
            crc1 = CRC.GetUSMBCRC16(buffer, (UInt16)bufferIndex);
            
            crc2 = Utility.BytesToUINT16(buffer, bufferIndex, false);
            bufferIndex += 2;

            if (crc1 != crc2)
            {
                return false;
            }
            
            if (buffer[bufferIndex] != 0xDD)
            {
                return false;
            }
            
            bufferIndex += 1;

            if (bufferLen != bufferIndex)
            {
                return false;
            }
            
            return true;
        }

        public void EncodeCmd()
        {
            return;
        }

        public void DecodeCmd()
        {
            return;
        }

        public void GetTaskModel(int devId, ref T_Tasks model)
        {
            model.CmdType = this.CmdType;
            model.CmdByte = this.CmdByte;
            model.DevId = devId;

            model.Status = 0;
            model.Length = (short)PacketLen;
            byte[] buffer = new byte[PacketLen];
            int bufferLen = 0;

            EncodeFrame(ref buffer, ref bufferLen);
            model.Data = buffer;

            model.SendTime = DateTime.Now;

            return;
        }
        #endregion
    }

    #region 心跳命令
    public class HeartCmd : ProtocolCmd
    {
        public HeartCmd()
        {
            this.CmdType = 0xF9;
            this.CmdByte = 0x1F;
        }
    }
    #endregion


    #region 时间校准
    public class TimeSyncCmd : ProtocolCmd
    {
        public TimeSyncCmd()
        {
            this.CmdType = 0xFB;
            this.CmdByte = 0x1F;
        }
        
        //public void EncodeCmd(RTCTime model)
        //{
        //    UInt16 bufferIndex = 0;

        //    Array.Copy(Utility.UINT16ToBytes(model.Year, false), 0, Data, bufferIndex, 2);
        //    bufferIndex += 2;

        //    Data[bufferIndex] = model.Month;
        //    bufferIndex += 1;

        //    Data[bufferIndex] = model.Day;
        //    bufferIndex += 1;

        //    Data[bufferIndex] = model.Hour;
        //    bufferIndex += 1;

        //    Data[bufferIndex] = model.Min;
        //    bufferIndex += 1;

        //    Data[bufferIndex] = model.Sec;
        //    bufferIndex += 1;

        //    DataLen += bufferIndex;
            
        //    return;
        //}
    }
    #endregion

    #region 设备控制命令
    public class DevCtrlCmd : ProtocolCmd
    {
        public DevCtrlCmd()
        {
            this.CmdType = 0xFC;
        }
        
        //读取CMP
        public void EncodeCMPReadCmd()
        {
            this.CmdByte = 0x1F;
            
            return;
        }
        
        //CMP周期设置
        public void EncodeCMPCycleSetCmd(UInt16 cycleTime)
        {
            this.CmdByte = 0x2F;
            UInt16 bufferIndex = 0;

            Utility.UINT16ToBytes(cycleTime, this.Data, bufferIndex, false);
            bufferIndex += 2;
            
            DataLen += bufferIndex;

            return;
        }

        //CPM停止
        public void EncodeCMStopCmd()
        {
            this.CmdByte = 0x3F;

            return;
        }

        //BG测试开始
        public void EncodeBGTestStartCmd()
        {
            this.CmdByte = 0x4F;

            return;
        }

        //BG测试停止
        public void EncodeBGTestStopCmd()
        {
            this.CmdByte = 0x5F;

            return;
        }

        //BG测试结果
        public void EncodeBGTestResultCmd()
        {
            this.CmdByte = 0x6F;

            return;
        }

        //SPAN测试开始
        public void EncodeSPANTestStartCmd()
        {
            this.CmdByte = 0x7F;

            return;
        }

        //SPAN测试停止
        public void EncodeSPANTestStopCmd()
        {
            this.CmdByte = 0x8F;

            return;
        }

        //SPAN测试结果
        public void EncodeSPANTestResultCmd()
        {
            this.CmdByte = 0x9F;

            return;
        }

        //开关量 OUT1 输出控制
        public void EncodeSetOUT1Cmd(byte state)
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0xAF;

            Data[bufferIndex] = state;
            bufferIndex += 1;
            
            DataLen += bufferIndex;
            
            return;
        }

        //开关量 OUT2 输出控制
        public void EncodeSetOUT2Cmd(byte state)
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0xBF;

            Data[bufferIndex] = state;
            bufferIndex += 1;

            DataLen += bufferIndex;

            return;
        }

        //主动上传瞬时值-开
        public void EncodeUpInstNoiseOpenCmd()
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0x1D;
            
            DataLen += bufferIndex;

            return;
        }

        //主动上传瞬时值-关
        public void EncodeUpInstNoiseCloseCmd()
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0x2D;
            
            DataLen += bufferIndex;

            return;
        }

        public void EncodeGpsInfoCmd()
        {
            CmdType = 0xFB;
            CmdByte = 0x2F;

            return;
        }
        
        //上传瞬时值
        public void EncodeUpInstNoiseCmd()
        {
            this.CmdByte = 0x3D;
            
            return;
        }

        //主动上传1S值-开
        public void EncodeUpOneSecNoiseOpenCmd()
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0x4D;
            
            DataLen += bufferIndex;

            return;
        }

        //主动上传1s值-关
        public void EncodeUpOneSecNoiseCloseCmd()
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0x5D;
            
            DataLen += bufferIndex;

            return;
        }

        //上传1s值
        public void EncodeUpOneSecNoiseCmd()
        {
            this.CmdByte = 0x6D;

            return;
        }

        //设为Z计权
        public void EncodeSetZWCmd()
        {
            this.CmdByte = 0x7D;

            return;
        }

        //设为C计权
        public void EncodeSetCWCmd()
        {
            this.CmdByte = 0x8D;

            return;
        }

        //设为A计权
        public void EncodeSetAWCmd()
        {
            this.CmdByte = 0x9D;

            return;
        }

        //设为F计权
        public void EncodeSetFGCmd()
        {
            this.CmdByte = 0xAD;

            return;
        }

        //设为S计权
        public void EncodeSetSGCmd()
        {
            this.CmdByte = 0xBD;

            return;
        }

        //设为I计权
        public void EncodeSetIGCmd()
        {
            this.CmdByte = 0xCD;

            return;
        }
        
        //写入新的灵敏度级
        //读出仪器当前的灵敏度级
        //读出仪器的校准结果
        //峰值C声级
        //峰值C声级清零

        //风向写入设备地址
        public void EncodeWindDirWriteDevAddrCmd(byte addr)
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0x1B;

            Data[bufferIndex] = addr;
            bufferIndex += 1;
            
            DataLen += bufferIndex;

            return;
        }

        //风向读取实时数据
        public void EncodeReadWindDirCmd(byte addr)
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0x2B;

            Data[bufferIndex] = addr;
            bufferIndex += 1;

            Utility.UINT16ToBytes(0x0000, this.Data, bufferIndex, false);
            bufferIndex += 2;

            Data[bufferIndex] = 0x01;
            bufferIndex += 1;

            DataLen += bufferIndex;
            
            return;
        }

        //风速写入设备地址
        public void EncodeWindSpeedWriteDevAddrCmd(byte addr)
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0x1A;

            Data[bufferIndex] = addr;
            bufferIndex += 1;

            DataLen += bufferIndex;

            return;
        }
        
        //风向读取实时数据
        public void EncodeReadWindSpeedCmd(byte addr)
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0x2A;

            Data[bufferIndex] = addr;
            bufferIndex += 1;

            Utility.UINT16ToBytes(0x0000, this.Data, bufferIndex, false);
            bufferIndex += 2;

            Data[bufferIndex] = 0x01;
            bufferIndex += 1;

            DataLen += bufferIndex;

            return;
        }

        //读取温湿度值
        public void EncodeReadESDataCmd()
        {
            this.CmdByte = 0x19;

            return;
        }
        
        public void EncodeReadAllDataCmd()
        {
            UInt16 bufferIndex = 0;
            this.CmdByte = 0x08;

            DataLen += bufferIndex;

            return;
        }

        public void EncodeSwitchAutoReport(UInt16 cycleTime)
        {
            this.CmdByte = 0x17;
            UInt16 bufferIndex = 0;

            Utility.UINT16ToBytes(cycleTime, this.Data, bufferIndex, false);
            bufferIndex += 2;

            DataLen += bufferIndex;

            return;
        }
    }
    #endregion

    #region 仪器上报&应答信息
    public struct ESData{
        public Byte PmState;
        public double Pm25;
        public double Pm100;
        public Byte CmpState;
        public double Cmp;
        public Byte NoiseState;
        public double Noise;
        public Byte WindDirState;
        public UInt16 WindDir;
        public Byte WindSpeedState;
        public double WindSpeed;
        public Byte ESState;
        public double Temperature;
        public double Humidity;
    };
    
    public class DevCtrlResponseCmd : ProtocolCmd
    {
        public DevCtrlResponseCmd()
        {
            this.CmdType = 0xFD;
        }

        public bool DecodeCMPReadCmd(ref double cmp)
        {
            int bufferIndex = 0;
            
            if (this.CmdByte != 0x1F)
            {
                return false;
            }
         
            if (this.DataLen != 2)
            {
                return false;
            }

            cmp = (double)(Utility.BytesToUINT16(this.Data, bufferIndex, false) / 1000.0);
            bufferIndex += 2;
            
            return true;   
        }

        public bool DecodeCMPCycleSetCmd(ref UInt16 cycleTime)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x2F)
            {
                return false;
            }

            if (this.DataLen != 2)
            {
                return false;
            }

            cycleTime = Utility.BytesToUINT16(this.Data, bufferIndex, false);
            bufferIndex += 2;

            return true;
        }

        //CPM停止
        public bool DecodeCMStopCmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x3F)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //BG测试开始
        public bool DecodeBGTestStartCmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x4F)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //BG测试停止
        public bool DecodeBGTestStopCmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x5F)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //BG测试结果
        public bool DecodeBGTestResultCmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x6F)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //SPAN测试开始
        public bool DecodeSPANTestStartCmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x7F)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //SPAN测试停止
        public bool DecodeSPANTestStopCmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x8F)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //SPAN测试结果
        public bool DecodeSPANTestResultCmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x9F)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //开关量 OUT1 输出控制
        public bool DecodeSetOUT1Cmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0xAF)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //开关量 OUT2 输出控制
        public bool DecodeSetOUT2Cmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0xBF)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = 1;
            bufferIndex += 1;

            return true;
        }

        //主动上传瞬时值-开
        public bool DecodeUpInstNoiseOpenCmd(ref double noise)
        {
            int bufferIndex = 0;
            UInt16 value;
            
            if (this.CmdByte != 0x1D)
            {
                return false;
            }

            if (this.DataLen != 2)
            {
                return false;
            }
            
            noise = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 10.0);
            bufferIndex += 2;

            return true;
        }

        //主动上传瞬时值-关
        public bool DecodeUpInstNoiseCloseCmd()
        {
            if (this.CmdByte != 0x2D)
            {
                return false;
            }

            if (this.DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //上传瞬时值
        public bool DecodeUpInstNoiseCmd(ref double noise)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x3D)
            {
                return false;
            }

            if (this.DataLen != 2)
            {
                return false;
            }

            noise = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 10.0);
            bufferIndex += 2;

            return true;
        }

        //主动上传1S值-开
        public bool DecodeUpOneSecNoiseOpenCmd(ref double noise)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x4D)
            {
                return false;
            }

            if (this.DataLen != 2)
            {
                return false;
            }

            noise = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 10.0);
            bufferIndex += 2;

            return true;
        }

        //主动上传1s值-关
        public bool DecodeUpOneSecNoiseCloseCmd()
        {
            if (this.CmdByte != 0x5D)
            {
                return false;
            }

            if (this.DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //上传1s值
        public bool DecodeUpOneSecNoiseCmd(ref double noise)
        {
            int bufferIndex = 0;
            
            if (this.CmdByte != 0x6D)
            {
                return false;
            }

            if (this.DataLen != 2)
            {
                return false;
            }

            noise = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 10.0);
            bufferIndex += 2;

            return true;
        }

        //设为Z计权
        public bool DecodeSetZWCmd()
        {
            if (this.CmdByte != 0x7D)
            {
                return false;
            }

            if (this.DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为C计权
        public bool DecodeSetCWCmd()
        {
            if (this.CmdByte != 0x8D)
            {
                return false;
            }

            if (this.DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为A计权
        public bool DecodeSetAWCmd()
        {
            if (this.CmdByte != 0x9D)
            {
                return false;
            }

            if (this.DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为F计权
        public bool DecodeSetFGCmd()
        {
            if (this.CmdByte != 0xAD)
            {
                return false;
            }

            if (this.DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为S计权
        public bool DecodeSetSGCmd()
        {
            if (this.CmdByte != 0xBD)
            {
                return false;
            }

            if (this.DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为I计权
        public bool DecodeSetIGCmd()
        {
            if (this.CmdByte != 0xCD)
            {
                return false;
            }

            if (this.DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //写入新的灵敏度级
        //读出仪器当前的灵敏度级
        //读出仪器的校准结果
        //峰值C声级
        //峰值C声级清零

        //风向写入设备地址
        public bool DecodeWindDirWriteDevAddrCmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x1B)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //风向读取实时数据
        public bool DecodeReadWindDirCmd(ref byte addr, ref UInt16 windDir)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x2B)
            {
                return false;
            }

            if (this.DataLen != 3)
            {
                return false;
            }

            addr = this.Data[bufferIndex];
            bufferIndex += 1;
            
            windDir = Utility.BytesToUINT16(this.Data, bufferIndex, false);
            bufferIndex += 2;

            return true;
        }

        //风速写入设备地址
        public bool DecodeWindSpeedWriteDevAddrCmd(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x1A)
            {
                return false;
            }

            if (this.DataLen != 1)
            {
                return false;
            }

            state = this.Data[bufferIndex];
            bufferIndex += 1;

            return true;
        }

        //风向读取实时数据
        public bool DecodeReadWindSpeedCmd(ref byte addr, ref double windSpeed)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x2A)
            {
                return false;
            }

            if (this.DataLen != 3)
            {
                return false;
            }

            addr = this.Data[bufferIndex];
            bufferIndex += 1;

            windSpeed = (double)(Utility.BytesToUINT16(this.Data, bufferIndex, false) / 10.0);
            bufferIndex += 2;

            return true;
        }

        //读取温湿度值
        public bool DecodeReadESDataCmd(ref double temperature, ref double humidity)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x19)
            {
                return false;
            }

            if (this.DataLen != 4)
            {
                return false;
            }

            temperature = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 100.0);
            bufferIndex += 2;

            humidity = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 100.0);
            bufferIndex += 2;

            return true;
        }

        public bool DecodeReadAllDataCmd(ref ESData model)
        {
            int bufferIndex = 0;

            CmdByte = 0x27;

            if (DataLen != 26)
            {
                return false;
            }

            UInt16 flag = Utility.BytesToUINT16(Data, bufferIndex, false);
            bufferIndex += 2;

            model.PmState = (Byte)((flag >> 5) & 0x01);
            model.CmpState = (Byte)((flag >> 4) & 0x01);
            model.NoiseState = (Byte)((flag >> 3) & 0x01);
            model.WindDirState = (Byte)((flag >> 2) & 0x01);
            model.WindSpeedState = (Byte)((flag >> 1) & 0x01);
            model.ESState = (Byte)(flag & 0x01);

            model.Pm25 = Utility.BytesToINT32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Pm100 = Utility.BytesToINT32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Cmp = Utility.BytesToINT32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Noise = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 10.0);
            bufferIndex += 2;

            //addr
            bufferIndex += 1;

            model.WindDir = Utility.BytesToUINT16(Data, bufferIndex, false);
            bufferIndex += 2;

            //addr
            bufferIndex += 1;

            model.WindSpeed = Utility.BytesToUINT16(Data, bufferIndex, false) / 10.0;
            bufferIndex += 2;

            model.Temperature = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 100.0);
            bufferIndex += 2;

            model.Humidity = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 100.0);
            bufferIndex += 2;

            return true;
        }

        public bool DecodeUpdateAllCmd(ref ESData model)
        {
            int bufferIndex = 0;

            CmdByte = 0x27;

            if (DataLen != 26)
            {
                return false;
            }

            UInt16 flag = Utility.BytesToUINT16(Data, bufferIndex, false);
            bufferIndex += 2;

            model.PmState = (Byte)((flag >> 5) & 0x01);
            model.CmpState = (Byte)((flag >> 4) & 0x01);
            model.NoiseState = (Byte)((flag >> 3) & 0x01);
            model.WindDirState = (Byte)((flag >> 2) & 0x01);
            model.WindSpeedState = (Byte)((flag >> 1) & 0x01);
            model.ESState = (Byte)(flag & 0x01);

            model.Pm25 = Utility.BytesToINT32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Pm100 = Utility.BytesToINT32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Cmp = Utility.BytesToINT32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Noise = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 10.0);
            bufferIndex += 2;

            //addr
            bufferIndex += 1;

            model.WindDir = Utility.BytesToUINT16(Data, bufferIndex, false);
            bufferIndex += 2;

            //addr
            bufferIndex += 1;

            model.WindSpeed = Utility.BytesToUINT16(Data, bufferIndex, false) / 10.0;
            bufferIndex += 2;

            model.Temperature = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 100.0);
            bufferIndex += 2;

            model.Humidity = (double)(this.Data[bufferIndex] + this.Data[bufferIndex + 1] / 100.0);
            bufferIndex += 2;

            return true;
        }
        public bool DecodeSwitchAutoReport(ref byte state)
        {
            int bufferIndex = 0;

            if (this.CmdByte != 0x17)
            {
                return false;
            }

            if (this.DataLen != 2)
            {
                return false;
            }
            state = 1;

            bufferIndex += 1;

            return true;
        }
    }
    #endregion

    
}
