using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace TestNLPIR
{
    [StructLayout(LayoutKind.Explicit)]
    public struct result_t
    {
        [FieldOffset(0)]
        public int start;
        [FieldOffset(4)]
        public int length;
        [FieldOffset(8)]
        public int sPos1;
        [FieldOffset(12)]
        public int sPos2;
        [FieldOffset(16)]
        public int sPos3;
        [FieldOffset(20)]
        public int sPos4;
        [FieldOffset(24)]
        public int sPos5;
        [FieldOffset(28)]
        public int sPos6;
        [FieldOffset(32)]
        public int sPos7;
        [FieldOffset(36)]
        public int sPos8;
        [FieldOffset(40)]
        public int sPos9;
        [FieldOffset(44)]
        public int sPos10;
        //[FieldOffset(12)] public int sPosLow;
        [FieldOffset(48)]
        public int POS_id;
        [FieldOffset(52)]
        public int word_ID;
        [FieldOffset(56)]
        public int word_type;
        [FieldOffset(60)]
        public double weight;
    }



    /// <summary>
    /// Class1 的摘要说明。
    /// </summary>
    class Class1
    {
        const string path = @"NLPIR.dll";//设定dll的路径
        //对函数进行申明
        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_Init")]
        public static extern bool NLPIR_Init(String sInitDirPath, int encoding);

        //特别注意，C语言的函数NLPIR_API const char * NLPIR_ParagraphProcess(const char *sParagraph,int bPOStagged=1);必须对应下面的申明
        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_ParagraphProcess")]
        public static extern IntPtr NLPIR_ParagraphProcess(String sParagraph, int bPOStagged = 1);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_Exit")]
        public static extern bool NLPIR_Exit();

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_ImportUserDict")]
        public static extern int NLPIR_ImportUserDict(String sFilename);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_FileProcess")]
        public static extern bool NLPIR_FileProcess(String sSrcFilename, String sDestFilename, int bPOStagged = 1);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_FileProcessEx")]
        public static extern bool NLPIR_FileProcessEx(String sSrcFilename, String sDestFilename);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_GetParagraphProcessAWordCount")]
        static extern int NLPIR_GetParagraphProcessAWordCount(String sParagraph);
        //NLPIR_GetParagraphProcessAWordCount
        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_ParagraphProcessAW")]
        static extern void NLPIR_ParagraphProcessAW(int nCount, [Out, MarshalAs(UnmanagedType.LPArray)] result_t[] result);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_AddUserWord")]
        static extern int NLPIR_AddUserWord(String sWord);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_SaveTheUsrDic")]
        static extern int NLPIR_SaveTheUsrDic();

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_DelUsrWord")]
        static extern int NLPIR_DelUsrWord(String sWord);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_Start")]
        static extern bool NLPIR_NWI_Start();

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_Complete")]
        static extern bool NLPIR_NWI_Complete();

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_AddFile")]
        static extern bool NLPIR_NWI_AddFile(String sText);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_AddMem")]
        static extern bool NLPIR_NWI_AddMem(String sText);

        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_NWI_GetResult")]
        public static extern IntPtr NLPIR_NWI_GetResult(bool bWeightOut = false);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_Result2UserDict")]
        static extern uint NLPIR_NWI_Result2UserDict();



        static void record_match(String[] result, String[] pattern)
        {
            
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //
            // TODO: 在此处添加代码以启动应用程序
            //
            if (!NLPIR_Init("", 0))//给出Data文件所在的路径，注意根据实际情况修改。
            {
                System.Console.WriteLine("Init ICTCLAS failed!");
                return;
            }
            System.Console.WriteLine("Init ICTCLAS success!");

            int import_count = NLPIR_ImportUserDict("userdict1.txt");
            Console.WriteLine("{0} uder dict item imported", import_count);
            String s = "9:30AM,前往同  济大学，堡胜吃盖浇饭花费10.3元。";
            s = s.Replace(" ",string.Empty);
            System.IO.StreamWriter file = new System.IO.StreamWriter("test.txt");
            file.WriteLine(s);
            
            int count = NLPIR_GetParagraphProcessAWordCount(s);//先得到结果的词数
            System.Console.WriteLine("NLPIR_GetParagraphProcessAWordCount success!");

            result_t[] result = new result_t[count];//在客户端申请资源
            NLPIR_ParagraphProcessAW(count, result);//获取结果存到客户的内存中
            int i = 1;
            foreach (result_t r in result)
            {
                String sWhichDic = "";
                switch (r.word_type)
                {
                    case 0:
                        sWhichDic = "核心词典";
                        break;
                    case 1:
                        sWhichDic = "用户词典";
                        break;
                    case 2:
                        sWhichDic = "专业词典";
                        break;
                    default:
                        break;
                }
                Console.WriteLine("No.{0}:start:{1}, length:{2},POS_ID:{3},Word_ID:{4}, UserDefine:{5}\n", i++, r.start, r.length, r.POS_id, r.word_ID, sWhichDic);//, s.Substring(r.start, r.length)
            }



            StringBuilder sResult = new StringBuilder(600);
            //准备存储空间        
            IntPtr intPtr = NLPIR_ParagraphProcess(s);//切分结果保存为IntPtr类型
            String str = Marshal.PtrToStringAnsi(intPtr);//将切分结果转换为string
            Console.WriteLine(str);
            file.WriteLine(str);
            String[] str_list = str.Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);
            List<SentenceSegment> segment_list = new List<SentenceSegment>();
            for (int j = 0; j < str_list.Length; j++)
            {
                String[] segment = str_list[j].Split('/');
                if (segment.Length < 2)
                {
                    file.WriteLine("\"" + str_list[j] + "\"" + "is not contain /");
                }
                else
                {
                    segment_list.Add(new SentenceSegment(){value = segment[0], type = segment[1]});
                }
            }

            foreach (var item in segment_list)
            {
                file.WriteLine(item);
            }

            PatternAction pa = new PatternAction("(/n?) /p? (/x) /v (/daxue) (/ct) /v (/n) 花费 (/m) (/q) {type:支出 user:/1 datetime:/2 position:/3+/4 cost:/6+/7}");

            System.Console.WriteLine("Begin Match");

            Account account = pa.match(segment_list.ToArray());
            if (account != null)
            {
                System.Console.WriteLine("Pattern matched!");
                file.WriteLine(account);
            }
            else {
                System.Console.WriteLine("Patter not matched!");
            }

            pa.print(file);

            //System.Console.WriteLine("Before Userdict imported:");
            //String ss;
            //Console.WriteLine("insert user dic:");
            //ss = Console.ReadLine();
            //while (ss[0] != 'q' && ss[0] != 'Q')
            //{
            //    //用户词典中添加词
            //    int iiii = NLPIR_AddUserWord(ss);//词 词性 example:点击下载 vyou
            //    intPtr = NLPIR_ParagraphProcess(s, 1);
            //    str = Marshal.PtrToStringAnsi(intPtr);
            //    System.Console.WriteLine(str);
            //    file.WriteLine(str);
            //    NLPIR_SaveTheUsrDic(); // save the user dictionary to the file

            //    //删除用户词典中的词
            //    Console.WriteLine("delete usr dic:");
            //    ss = Console.ReadLine();
            //    iiii = NLPIR_DelUsrWord(ss);
            //    str = Marshal.PtrToStringAnsi(intPtr);
            //    System.Console.WriteLine(str);
            //    NLPIR_SaveTheUsrDic();

            //}


            //测试新词发现与自适应分词功能
            //NLPIR_NWI_Start();//新词发现功能启动
            //NLPIR_NWI_AddFile("test/test.txt");//添加一个待发现新词的文件，可反复添加

            //NLPIR_NWI_Complete();//新词发现完成


            //intPtr = NLPIR_NWI_GetResult();
            //str = Marshal.PtrToStringAnsi(intPtr);


            //file.WriteLine("新词识别结果:");
            //file.WriteLine(str);
            //NLPIR_FileProcess("test/test.txt","test/test.txt");
            //NLPIR_NWI_Result2UserDict();//新词识别结果导入分词库
            //NLPIR_FileProcess("test/test.txt", "test/test.txt");
            NLPIR_Exit();
            file.Close();
        }
    }
}
