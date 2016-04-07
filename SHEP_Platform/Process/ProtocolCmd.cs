using System;
using Web.Common;

namespace SHEP_Platform.Process
{
    
    public class ProtocolCmd
    {

        public ProtocolCmd()
        {
            NodeId = 0;
            CmdType = 0;
            CmdByte = 0;
            SrcAddr= 0;
            DstAddr = 0;
            DataLen = 0;
        }

        #region Model

        /// <summary>
        /// 节点Id
        /// </summary>
        public uint NodeId { set; get; }

        /// <summary>
        /// 命令码
        /// </summary>
        public byte CmdType { set; get; }

        /// <summary>
        /// 命令码
        /// </summary>
        public byte CmdByte { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public byte[] Pwd { set; get; } = new byte[8];

        /// <summary>
        /// 描述
        /// </summary>
        public byte[] Description { set; get; } = new byte [12];

        /// <summary>
        /// 源地址
        /// </summary>
        public byte SrcAddr { set; get; }

        /// <summary>
        /// 目标地址
        /// </summary>
        public byte DstAddr { set; get; }

        /// <summary>
        /// 发送数据
        /// </summary>
        public byte[] Data { set; get; } = new byte[512];

        /// <summary>
        /// 发送数据长度
        /// </summary>
        public ushort DataLen { set; get; }

        public ushort PacketLen => (ushort)(DataLen + 34);

        #endregion
        
        #region 命令
        public void EncodeFrame(ref byte[] buffer, ref int bufferLen)
        {
            if (bufferLen <= 0) throw new ArgumentOutOfRangeException(nameof(bufferLen));

            ushort bufferIndex = 0;

            buffer[bufferIndex] = 0xAA;
            bufferIndex += 1;

            buffer[bufferIndex] = CmdType;
            bufferIndex += 1;

            buffer[bufferIndex] = CmdByte;
            bufferIndex += 1;
            
            Array.Copy(Pwd, 0, buffer, bufferIndex, 8);
            bufferIndex += 8;

            Utility.Uint32ToBytes(NodeId, buffer, bufferIndex, false);
            bufferIndex += 4;

            Array.Copy(Description, 0, buffer, bufferIndex, 12);
            bufferIndex += 12;
            
            SrcAddr = 0x00;
            
            buffer[bufferIndex] = SrcAddr;
            bufferIndex += 1;

            DstAddr = 0x01;
            buffer[bufferIndex] = DstAddr;
            bufferIndex += 1;

            Utility.Uint16ToBytes(DataLen, buffer, bufferIndex, false);
            bufferIndex += 2;

            Array.Copy(Data, 0, buffer, bufferIndex, DataLen);
            bufferIndex += DataLen;

            //CRC 
            var crc = CRC.GetUSMBCRC16(buffer, bufferIndex);

            Utility.Uint16ToBytes(crc, buffer, bufferIndex, false);
            //Array.Copy(buffer, bufferIndex, Utility.Uint16ToBytes(crc, false), 0, 2);
            bufferIndex += 2;

            buffer[bufferIndex] = 0xDD;
            bufferIndex += 1;

            bufferLen = bufferIndex;
        }

        public bool DecodeFrame(byte[] buffer, int bufferLen)
        {
            ushort bufferIndex = 0;

            if (buffer[bufferIndex] != 0xAA)
            {
                return false;
            }
            bufferIndex += 1;

            CmdType = buffer[bufferIndex];
            bufferIndex += 1;

            CmdByte = buffer[bufferIndex];
            bufferIndex += 1;

            Array.Copy(buffer, bufferIndex, Pwd, 0, 8);
            bufferIndex += 8;

            NodeId = Utility.BytesToUint32(buffer, bufferIndex, false);
            bufferIndex += 4;

            Array.Copy(buffer, bufferIndex, Description, 0, 12);
            bufferIndex += 12;

            SrcAddr = buffer[bufferIndex];
            bufferIndex += 1;

            DstAddr = buffer[bufferIndex];
            bufferIndex += 1;

            DataLen = Utility.BytesToUint16(buffer, bufferIndex, false);
            bufferIndex += 2;
            
            Array.Copy(buffer, bufferIndex, Data, 0, DataLen);
            bufferIndex += DataLen;
            
            var crc1 = CRC.GetUSMBCRC16(buffer, bufferIndex);
            
            var crc2 = Utility.BytesToUint16(buffer, bufferIndex, false);
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
        }

        public void DecodeCmd()
        {
        }

        public void GetTaskModel(int devId, ref T_Tasks model)
        {
            model.CmdType = CmdType;
            model.CmdByte = CmdByte;
            model.DevId = devId;

            model.Status = 0;
            model.Length = (short)PacketLen;
            var buffer = new byte[PacketLen];
            var bufferLen = 0;

            EncodeFrame(ref buffer, ref bufferLen);
            model.Data = buffer;

            model.SendTime = DateTime.Now;
        }
        #endregion
    }

    #region 心跳命令
    public class HeartCmd : ProtocolCmd
    {
        public HeartCmd()
        {
            CmdType = 0xF9;
            CmdByte = 0x1F;
        }
    }
    #endregion


    #region 时间校准
    public class TimeSyncCmd : ProtocolCmd
    {
        public TimeSyncCmd()
        {
            CmdType = 0xFB;
            CmdByte = 0x1F;
        }
    }
    #endregion

    #region 设备控制命令
    public class DevCtrlCmd : ProtocolCmd
    {
        public DevCtrlCmd()
        {
            CmdType = 0xFC;
        }
        
        //读取CMP
        public void EncodeCmpReadCmd()
        {
            CmdByte = 0x1F;
        }
        
        //CMP周期设置
        public void EncodeCmpCycleSetCmd(ushort cycleTime)
        {
            CmdByte = 0x2F;
            ushort bufferIndex = 0;

            Utility.Uint16ToBytes(cycleTime, Data, bufferIndex, false);
            bufferIndex += 2;
            
            DataLen += bufferIndex;
        }

        //CPM停止
        public void EncodeCmStopCmd()
        {
            CmdByte = 0x3F;
        }

        //BG测试开始
        public void EncodeBgTestStartCmd()
        {
            CmdByte = 0x4F;
        }

        //BG测试停止
        public void EncodeBgTestStopCmd()
        {
            CmdByte = 0x5F;
        }

        //BG测试结果
        public void EncodeBgTestResultCmd()
        {
            CmdByte = 0x6F;
        }

        //SPAN测试开始
        public void EncodeSpanTestStartCmd()
        {
            CmdByte = 0x7F;
        }

        //SPAN测试停止
        public void EncodeSpanTestStopCmd()
        {
            CmdByte = 0x8F;
        }

        //SPAN测试结果
        public void EncodeSpanTestResultCmd()
        {
            CmdByte = 0x9F;
        }

        //开关量 OUT1 输出控制
        public void EncodeSetOut1Cmd(byte state)
        {
            ushort bufferIndex = 0;
            CmdByte = 0xAF;

            Data[bufferIndex] = state;
            bufferIndex += 1;
            
            DataLen += bufferIndex;
        }

        //开关量 OUT2 输出控制
        public void EncodeSetOut2Cmd(byte state)
        {
            ushort bufferIndex = 0;
            CmdByte = 0xBF;

            Data[bufferIndex] = state;
            bufferIndex += 1;

            DataLen += bufferIndex;
        }

        //主动上传瞬时值-开
        public void EncodeUpInstNoiseOpenCmd()
        {
            ushort bufferIndex = 0;
            CmdByte = 0x1D;
            
            DataLen += bufferIndex;
        }

        //主动上传瞬时值-关
        public void EncodeUpInstNoiseCloseCmd()
        {
            ushort bufferIndex = 0;
            CmdByte = 0x2D;
            
            DataLen += bufferIndex;
        }

        public void EncodeGpsInfoCmd()
        {
            CmdType = 0xFB;
            CmdByte = 0x2F;
        }
        
        //上传瞬时值
        public void EncodeUpInstNoiseCmd()
        {
            CmdByte = 0x3D;
        }

        //主动上传1S值-开
        public void EncodeUpOneSecNoiseOpenCmd()
        {
            ushort bufferIndex = 0;
            CmdByte = 0x4D;
            
            DataLen += bufferIndex;
        }

        //主动上传1s值-关
        public void EncodeUpOneSecNoiseCloseCmd()
        {
            ushort bufferIndex = 0;
            CmdByte = 0x5D;
            
            DataLen += bufferIndex;
        }

        //上传1s值
        public void EncodeUpOneSecNoiseCmd()
        {
            CmdByte = 0x6D;
        }

        //设为Z计权
        public void EncodeSetZwCmd()
        {
            CmdByte = 0x7D;
        }

        //设为C计权
        public void EncodeSetCwCmd()
        {
            CmdByte = 0x8D;
        }

        //设为A计权
        public void EncodeSetAwCmd()
        {
            CmdByte = 0x9D;
        }

        //设为F计权
        public void EncodeSetFgCmd()
        {
            CmdByte = 0xAD;
        }

        //设为S计权
        public void EncodeSetSgCmd()
        {
            CmdByte = 0xBD;
        }

        //设为I计权
        public void EncodeSetIgCmd()
        {
            CmdByte = 0xCD;
        }
        
        //写入新的灵敏度级
        //读出仪器当前的灵敏度级
        //读出仪器的校准结果
        //峰值C声级
        //峰值C声级清零

        //风向写入设备地址
        public void EncodeWindDirWriteDevAddrCmd(byte addr)
        {
            ushort bufferIndex = 0;
            CmdByte = 0x1B;

            Data[bufferIndex] = addr;
            bufferIndex += 1;
            
            DataLen += bufferIndex;
        }

        //风向读取实时数据
        public void EncodeReadWindDirCmd(byte addr)
        {
            ushort bufferIndex = 0;
            CmdByte = 0x2B;

            Data[bufferIndex] = addr;
            bufferIndex += 1;

            Utility.Uint16ToBytes(0x0000, Data, bufferIndex, false);
            bufferIndex += 2;

            Data[bufferIndex] = 0x01;
            bufferIndex += 1;

            DataLen += bufferIndex;
        }

        //风速写入设备地址
        public void EncodeWindSpeedWriteDevAddrCmd(byte addr)
        {
            ushort bufferIndex = 0;
            CmdByte = 0x1A;

            Data[bufferIndex] = addr;
            bufferIndex += 1;

            DataLen += bufferIndex;
        }
        
        //风向读取实时数据
        public void EncodeReadWindSpeedCmd(byte addr)
        {
            ushort bufferIndex = 0;
            CmdByte = 0x2A;

            Data[bufferIndex] = addr;
            bufferIndex += 1;

            Utility.Uint16ToBytes(0x0000, Data, bufferIndex, false);
            bufferIndex += 2;

            Data[bufferIndex] = 0x01;
            bufferIndex += 1;

            DataLen += bufferIndex;
        }

        //读取温湿度值
        public void EncodeReadEsDataCmd()
        {
            CmdByte = 0x19;
        }
        
        public void EncodeReadAllDataCmd()
        {
            ushort bufferIndex = 0;
            CmdByte = 0x08;

            DataLen += bufferIndex;
        }

        public void EncodeSwitchAutoReport(ushort cycleTime)
        {
            CmdByte = 0x17;
            ushort bufferIndex = 0;

            Utility.Uint16ToBytes(cycleTime, Data, bufferIndex, false);
            bufferIndex += 2;

            DataLen += bufferIndex;
        }
    }
    #endregion

    #region 仪器上报&应答信息
    public struct EsData{
        public byte PmState;
        public double Pm25;
        public double Pm100;
        public byte CmpState;
        public double Cmp;
        public byte NoiseState;
        public double Noise;
        public byte WindDirState;
        public ushort WindDir;
        public byte WindSpeedState;
        public double WindSpeed;
        public byte EsState;
        public double Temperature;
        public double Humidity;
    }
    
    public class DevCtrlResponseCmd : ProtocolCmd
    {
        public DevCtrlResponseCmd()
        {
            CmdType = 0xFD;
        }

        public bool DecodeCmpReadCmd(ref double cmp)
        {
            var bufferIndex = 0;
            
            if (CmdByte != 0x1F)
            {
                return false;
            }
         
            if (DataLen != 2)
            {
                return false;
            }

            cmp = Utility.BytesToUint16(Data, bufferIndex, false) / 1000.0;

            return true;   
        }

        public bool DecodeCmpCycleSetCmd(ref ushort cycleTime)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x2F)
            {
                return false;
            }

            if (DataLen != 2)
            {
                return false;
            }

            cycleTime = Utility.BytesToUint16(Data, bufferIndex, false);

            return true;
        }

        //CPM停止
        public bool DecodeCmStopCmd(ref byte state)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x3F)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //BG测试开始
        public bool DecodeBgTestStartCmd(ref byte state)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x4F)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //BG测试停止
        public bool DecodeBgTestStopCmd(ref byte state)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x5F)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //BG测试结果
        public bool DecodeBgTestResultCmd(ref byte state)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x6F)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //SPAN测试开始
        public bool DecodeSpanTestStartCmd(ref byte state)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x7F)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //SPAN测试停止
        public bool DecodeSpanTestStopCmd(ref byte state)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x8F)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //SPAN测试结果
        public bool DecodeSpanTestResultCmd(ref byte state)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x9F)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //开关量 OUT1 输出控制
        public bool DecodeSetOut1Cmd(ref byte state)
        {
            var bufferIndex = 0;

            if (CmdByte != 0xAF)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //开关量 OUT2 输出控制
        public bool DecodeSetOut2Cmd(ref byte state)
        {
            if (CmdByte != 0xBF)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = 1;

            return true;
        }

        //主动上传瞬时值-开
        public bool DecodeUpInstNoiseOpenCmd(ref double noise)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x1D)
            {
                return false;
            }

            if (DataLen != 2)
            {
                return false;
            }
            
            noise = Data[bufferIndex] + Data[bufferIndex + 1] / 10.0;

            return true;
        }

        //主动上传瞬时值-关
        public bool DecodeUpInstNoiseCloseCmd()
        {
            if (CmdByte != 0x2D)
            {
                return false;
            }

            if (DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //上传瞬时值
        public bool DecodeUpInstNoiseCmd(ref double noise)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x3D)
            {
                return false;
            }

            if (DataLen != 2)
            {
                return false;
            }

            noise = Data[bufferIndex] + Data[bufferIndex + 1] / 10.0;

            return true;
        }

        //主动上传1S值-开
        public bool DecodeUpOneSecNoiseOpenCmd(ref double noise)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x4D)
            {
                return false;
            }

            if (DataLen != 2)
            {
                return false;
            }

            noise = Data[bufferIndex] + Data[bufferIndex + 1] / 10.0;

            return true;
        }

        //主动上传1s值-关
        public bool DecodeUpOneSecNoiseCloseCmd()
        {
            if (CmdByte != 0x5D)
            {
                return false;
            }

            if (DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //上传1s值
        public bool DecodeUpOneSecNoiseCmd(ref double noise)
        {
            var bufferIndex = 0;
            
            if (CmdByte != 0x6D)
            {
                return false;
            }

            if (DataLen != 2)
            {
                return false;
            }

            noise = Data[bufferIndex] + Data[bufferIndex + 1] / 10.0;

            return true;
        }

        //设为Z计权
        public bool DecodeSetZwCmd()
        {
            if (CmdByte != 0x7D)
            {
                return false;
            }

            if (DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为C计权
        public bool DecodeSetCwCmd()
        {
            if (CmdByte != 0x8D)
            {
                return false;
            }

            if (DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为A计权
        public bool DecodeSetAwCmd()
        {
            if (CmdByte != 0x9D)
            {
                return false;
            }

            if (DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为F计权
        public bool DecodeSetFgCmd()
        {
            if (CmdByte != 0xAD)
            {
                return false;
            }

            if (DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为S计权
        public bool DecodeSetSgCmd()
        {
            if (CmdByte != 0xBD)
            {
                return false;
            }

            if (DataLen != 0)
            {
                return false;
            }

            return true;
        }

        //设为I计权
        public bool DecodeSetIgCmd()
        {
            if (CmdByte != 0xCD)
            {
                return false;
            }

            if (DataLen != 0)
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
            var bufferIndex = 0;

            if (CmdByte != 0x1B)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //风向读取实时数据
        public bool DecodeReadWindDirCmd(ref byte addr, ref ushort windDir)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x2B)
            {
                return false;
            }

            if (DataLen != 3)
            {
                return false;
            }

            addr = Data[bufferIndex];
            bufferIndex += 1;
            
            windDir = Utility.BytesToUint16(Data, bufferIndex, false);

            return true;
        }

        //风速写入设备地址
        public bool DecodeWindSpeedWriteDevAddrCmd(ref byte state)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x1A)
            {
                return false;
            }

            if (DataLen != 1)
            {
                return false;
            }

            state = Data[bufferIndex];

            return true;
        }

        //风向读取实时数据
        public bool DecodeReadWindSpeedCmd(ref byte addr, ref double windSpeed)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x2A)
            {
                return false;
            }

            if (DataLen != 3)
            {
                return false;
            }

            addr = Data[bufferIndex];
            bufferIndex += 1;

            windSpeed = Utility.BytesToUint16(Data, bufferIndex, false) / 10.0;

            return true;
        }

        //读取温湿度值
        public bool DecodeReadEsDataCmd(ref double temperature, ref double humidity)
        {
            var bufferIndex = 0;

            if (CmdByte != 0x19)
            {
                return false;
            }

            if (DataLen != 4)
            {
                return false;
            }

            temperature = Data[bufferIndex] + Data[bufferIndex + 1] / 100.0;
            bufferIndex += 2;

            humidity = Data[bufferIndex] + Data[bufferIndex + 1] / 100.0;

            return true;
        }

        public bool DecodeReadAllDataCmd(ref EsData model)
        {
            var bufferIndex = 0;

            CmdByte = 0x27;

            if (DataLen != 26)
            {
                return false;
            }

            var flag = Utility.BytesToUint16(Data, bufferIndex, false);
            bufferIndex += 2;

            model.PmState = (byte)((flag >> 5) & 0x01);
            model.CmpState = (byte)((flag >> 4) & 0x01);
            model.NoiseState = (byte)((flag >> 3) & 0x01);
            model.WindDirState = (byte)((flag >> 2) & 0x01);
            model.WindSpeedState = (byte)((flag >> 1) & 0x01);
            model.EsState = (byte)(flag & 0x01);

            model.Pm25 = Utility.BytesToInt32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Pm100 = Utility.BytesToInt32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Cmp = Utility.BytesToInt32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Noise = Data[bufferIndex] + Data[bufferIndex + 1] / 10.0;
            bufferIndex += 2;

            //addr
            bufferIndex += 1;

            model.WindDir = Utility.BytesToUint16(Data, bufferIndex, false);
            bufferIndex += 2;

            //addr
            bufferIndex += 1;

            model.WindSpeed = Utility.BytesToUint16(Data, bufferIndex, false) / 10.0;
            bufferIndex += 2;

            model.Temperature = Data[bufferIndex] + Data[bufferIndex + 1] / 100.0;
            bufferIndex += 2;

            model.Humidity = Data[bufferIndex] + Data[bufferIndex + 1] / 100.0;

            return true;
        }

        public bool DecodeUpdateAllCmd(ref EsData model)
        {
            var bufferIndex = 0;

            CmdByte = 0x27;

            if (DataLen != 26)
            {
                return false;
            }

            var flag = Utility.BytesToUint16(Data, bufferIndex, false);
            bufferIndex += 2;

            model.PmState = (byte)((flag >> 5) & 0x01);
            model.CmpState = (byte)((flag >> 4) & 0x01);
            model.NoiseState = (byte)((flag >> 3) & 0x01);
            model.WindDirState = (byte)((flag >> 2) & 0x01);
            model.WindSpeedState = (byte)((flag >> 1) & 0x01);
            model.EsState = (byte)(flag & 0x01);

            model.Pm25 = Utility.BytesToInt32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Pm100 = Utility.BytesToInt32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Cmp = Utility.BytesToInt32(Data, bufferIndex, false);
            bufferIndex += 4;

            model.Noise = Data[bufferIndex] + Data[bufferIndex + 1] / 10.0;
            bufferIndex += 2;

            //addr
            bufferIndex += 1;

            model.WindDir = Utility.BytesToUint16(Data, bufferIndex, false);
            bufferIndex += 2;

            //addr
            bufferIndex += 1;

            model.WindSpeed = Utility.BytesToUint16(Data, bufferIndex, false) / 10.0;
            bufferIndex += 2;

            model.Temperature = Data[bufferIndex] + Data[bufferIndex + 1] / 100.0;
            bufferIndex += 2;

            model.Humidity = Data[bufferIndex] + Data[bufferIndex + 1] / 100.0;

            return true;
        }
        public bool DecodeSwitchAutoReport(ref byte state)
        {
            if (CmdByte != 0x17)
            {
                return false;
            }

            if (DataLen != 2)
            {
                return false;
            }
            state = 1;

            return true;
        }
    }
    #endregion

    
}
