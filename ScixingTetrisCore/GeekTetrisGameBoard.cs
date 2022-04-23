using ScixingTetrisCore.Interface;
using ScixingTetrisCore.Rule;
using ScixingTetrisCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class GeekTetrisGameBoard : ITetrisGameBoard
    {
        string gg = "ZIOSLZSIZJSSZZIZZJOLSOLLSZSJSZZIZZSJZOZSZOZZOLOZJJOIZLZSSSTSOIZOZJZZOZLZZOZOSSZZSZOTLTIZSSZLTSLTIZZJOLSZSOISZLSITZOJSZZTSTZTSTLOZOOJSTTOJZTZIOZOTZOTZTTSTZZJTZJOOOSLTSOTTTOSJTZJSJTSZLIZZZTTZOOTZZLTOZSOZTILLTSSSLITTTSJISTJZZJZZLLZTLLTZTZTOLJZZOZLZOSSSLISLTJSJLSTIZTTJTTTTSIZZZOLTLZOSITSOLOTTJSOSZZTZZIZZIILOLZOIZZZSTOZOOLTTSZZZLSITITZSTJZOJILJOSTZOLOOOIJTJOOZLZOOZTSTTISSSLTTIZSLOOJTOSITIZZLIJSSLOOIITSSOZLOJTTIOLSLTJOSZSSTZSOTTSLZILZZZTIJSZZZZZOZSTZZSZJZTSSTOSSSIZSTLSJLTZSTZOJSOSSLZSJSJLSSZTTOZTZSOOTJZZSOTSZTZOJOZJZSSSZZSZTZSSZOSTSSLJIZJSZZTLJSLSZTTITZOLOZOOLOZSZLLSLZSOZZLZOZSSZSOZZJZZOSTSOSSSZSIOTTSTZTOLZSOSJSZOZLSOOISLOOLJOZJZJSZLJTSLLTOSJTZZLTZOTSZZSIZZLZSZOTZOJSOTTSSSSSLTTZOZTZLTSJLOLSJZOZTOOITLOZILSZLOZSSSJTTTZSSZZSSOZSOSLJTOJJTOJZSIOTOZOSZZJZZOSLSJOLTZTOSSOZIZZZTJSLSILOJOSOLZOLJZOZZLZISSSISSTISOILZOJSSTOOOJOJZTLTSLSJTZSSISTSTTZTOJZSSZJOOSLTJTLSSTTZSZZZOZJJJOSSSTSZSTZTZIJTIZZSSOZLTTILSSJOZZSZZTSLOOZSSTTJSSZOIZJOSOJTTLZSZZZOTSLSSJIJTTSZZZSTOTOOSZLZIZZISTSZTJOOIZJSTTZJLOISJSSZZIJSJIOJZOJSZJSLIJTZOTSZSOTLJZIJOZOISTZZSZOIJLSJZTLZSSOSITISSSOOLZSZOSTOZOZLSZOOOZSJJJSLTSISISIOOLJZSZOOZZLSZSOZSZJTZZZIZLZZJJSTSOOSOOZJZLJSTLSTSZSSOTLSOISOJSTTJSSJOSJOOJLSOTIISJJZSISSSOLTIOTJSZZLTIZLSOSZZSTOSJZTJTZJZOZJSOJZLIJSLLSZSJITOSTZTTSZIOOOOLISZTOSZZOJJLOTLOTTOJJTZZSSIJIOSZILZZTZIZOLZLZZSZZSISIJLZIOTOLOSTOZOLJZOOZSOZSJISOZTZJZTIZSTITTSOSLLTLJIZOZZSJJSZZSZLZZOSJOJLOOJOTSZOTLTZZSTOZSZZSTZOZSOISSJLTTLZLJTTLSZZSSJIJJSTSJSIZSZOZLLTSSOTJIZSOOSTZJZJSZJTJOOOTSOLLZLSOTTZJTSOJZOSZOSJLZSIOTOSZLTSILZSJZTOOOZZZJOSJTSTOZOOIOLTTSLSTTSTJOZISTZZLSOLTOZZZTTOIOSTJTTOZSZSSLJZTZOZZTZSOSOOZOLSZZOLJSTSSTOITZTLZSJJIJSTTJSZTOIOOSZSTOJJJOZLJTTTITZOZLISSSSIZTZZTILSZSZZZZSSZTLZTTTTOIJLSTZLOJZOOLZZSOZOOIOJZJZTSOLISLJISZSLLSOSSJJOOLTSZTOOOSZTTZLJTISOTZZTJTLJOZJOOTSZTSIZZZOSISSSJILLZTOOJZZSSZSZOOSSSTOZJZOZZZZSZJSOOZSOOZIZJZTSJSJTJOZOOZTJILIOZZLSSZTOLZIOZZJTSSLITJJLOLTSLLJZSOSTZZJZOOOOISSOLILOJIIIZOJOJOZSTJZTOZJSJOZIOSZSIOZTTLTLZOJOZZTJOZJTJSSJZSIJTZZITZLZJZZZSSOTOOSJIIZTZSSSJSOITLZZSLSSSOJJOTSIOSSZZOTLZSTITZTZTOTTLISIZOZJTTLJJOJSZOISZJZSLLZJSZOOZZSSJOOZLZSSSOOTSZZSZSIZSTSOTLIOSLJZOZTJSOTJSLOJJTOSJSLSLSJSZTOSOJTIOSJLZZZLJJLOJSTOOSZZJIOJSZLSSSOSSSTOZZSJZTZZZSZSLLJOSSLTITJOLZZJZLOZJJOLOZSJZSTZLSIZSOSITOLZZZLZTIJZTOIZZISIOJZZJTSSZSLOSZZZIZSJSOTTIZOOZOLSSIIJLSLIITSSILJZOSSTZLISZZZSJLSLSTZJZSOTSZOJOZTJIZOOZJJSOOOTZLOTTOOSZOOLOSSLOSSZSSTLSZISOOSZIJJZSIILOTZTOSTOZJZJSZOZTOOZOZJOSIZSOOZOJZSSZTOSTTOJZOTSJOLJSOLTLLOZZTSZZZZZTSLZZLOOOZZTTTSOSSSZTOOTIJSJTTZSJZOOLZOTJSZTZLTTSIJZTSOSSZZSOZZZZSLISSJISJJLOZZOJJTSTTZZOSIOLSZLOZOZTLZLZSTSSOTOLSZZSIOOSTJOZSLZJSOSSSOOJSZLOOZJOOOTSSLSTOZTTTJZOJSSLTSIOZZLOILJOIZSZLLZJITOSJJJZLZZOITTTSOZTZSJZSSOJZJLZZOTZITSOLLZSJSLZZZZTTZZZTOJSSIZTTLSTOTISSSZLZOZOLSTZTZZZZOITJLTSTZSIZLLJOJSLOJZOSZOZOSJJZOTZISSSTTSZOZSTJJLTOZSZZOSISSOSZZSSSZTLOZJJOZOSOSZSSOSZTSLSITLTZJOJSOOZSOJOIZIIZLTOJSLTSLOOZSLZZZZSTZZJTOTTSSISOZZOTTZSLJOZZZLSZSJISTSZTZSZZZTSISTTJZLLLZJIJLSJZSZSJLZLLOJTZTZSTTSOLLLZOJIZTTTLZTTZLZTJISOTSSSZJSLJJSSOTLSZZOTTOOLSIIOSJTJZTSSLOZOOZSOOZTOJZTJSJJJOSOLZOSLIIZOSZOTOZLOITSSTJOTJTJSSOSZJTIOSJZSTSZJLIITIZZSSZSSSSSSSOZOSLSOSSZTJZLLOLJISSOJZSZSTSIITSLTSJOZZJSLJZJLSOZOJLZTSJLSIZTJSIZIOJTSOZOLLJZJSSTSSTOTOZZTLJSTZZZZZJZTTTSZOSOTOLZSZZTSIJJTZOZSOOSOISOSZSOOOZSZTZZZSTSTIZTLSZZTOITOSLOJZSJSJTZLSZOSZOIOOZZSTTLSLSZJZJJZSZZTZOZZZOJSTOZSZSZISTSSZSZZTIZOOTZOIOSIOLZOSZILSJOJOLZLLOLOZZJJZOSZLSZJSLSZISZJSOOSSZIJSJIZIOISLIJZLSTLOSZJZOISLTOZLJSZOJLOOJSOSTSOOOIIOOIJZLISZOZSLLSSSZSZSOZZZJTSZOZLTZSZIIZOTTSJTTSOTIJOJOIOSLSJLJSLOZSSOZSOJZIIITJTTSIISOIZLSZJIOOJLTLTZOSISLJZTLSOOOSSJZOZJZOJSJLZIZZSSOOTOZOZSTLOJSLTSZJLSZOSJSSIZSTISTSZLSOSJJSITLJLOJOZTOTSOSTTSISSIIITTSTLOSISSJJZSOTOTOIITTSZSOSLZJOJJZZZIIZZISTSSOISZZSZLOTJJSOSIOJSOZZSTZOJZTIZTIOSZZZTZOZSLLTJOSISZSZTSOZZOSOITISSSZTLTLLSSOZSSSTZTIOZTJZSZOZLTOOOJSIOZZSSSOIOSLTSLOSSOZOSOLTZIZSSJTOTSTOLSLSZZSSIZLSOTZTZLOZZSLSSJSIZZLLOZZSLZJZSLOOIOZZJZZTLTZOJITSSZSJZOTOISLZZSJSSZLZIZOTZOOSSOSSTOSTSJLSLJSOSOTZTTZTZTZSLJJILSZJOZJILZZJLZOJLJTSJISSZOLSISZSZZOTSJLJSSSOISSOILIZSZTJZOSTJTSTSJZTZLTSOTOZJOSSIOSTLZLSIIZSSLLJJOSSZLLSSOZLOOLTOZTILOOJZOZZSJSZZSSJOSTZSOLOSOTOSZSZSZSOOSSLTZOZZJLZJJOOJJOZLZITSSLZTOLSITSZTSLLZJSTOIOOSSZZSIOJZTSIOJSSZIOLTOJJJJZSSLTOJOZZTZSISZZTSZOZZSISTOTTOOTOTZOIZZSZLSJOITIOZSSIZLSJOJZZLLLSSZLLSZSLSTIZLTLLLZZZJOTZOZLTTZOJILLZJZSLITOZZSLZZISOLISOSZOSTLJLSOOOZZJTZTLZZLSJOTZZOIJTZJOLOSIIZSOSLJTJOSSTIZSJZJLTTJSISZZOOOTOLZOTSSLZZLSJOSSSJOTIZIIZZTTSJOSZLOSOZTITZSTZSZZISITJTOJLZOOITZTJOLSOSZISSSSTOOTSOZSISSSSZSOIOSTTJJJSLOZJZOLTSLLOSSIJZJZSLLOTZTIJOSSOOTSSZOJSJZIOJZLOZZOIJZLZSJIZOJTTZLTOSIOIJOZSTLJLZZIZTZZSJISIZZJSOOOLJJTZZZZZZSLJIZSTZZOOJOZSOSSTIZJTSLZSSSJSTZOIOTJZOLLZTJSOSLOZJSSOTJTZSLOSLJSZJSTOOOLOTIOTOSZLZIOSLZZZZLSZSLOSOZOSSSSLLJLLZITTSTZOOSLJSTJSSISZSZTOTOOLOZSOTLJSTTOZZOZZTOTSSSTOJOTZSSZZLTLJSLSSOTSSSZOZLSJTOOZTZZIJTOZTSSJSIOSLZJOZZSISTTILTTOSISOLSTTTOTOLLIOLSSOOTZOOOOZZJZSSSZOZJSOZZZZLLJIZOZSZTZZISLIOSJSOOSLSTOIZTOZOZOJIJZOZOZOZTISTZZLTJSZZOJSSZOOTOJOZTJLITZLTZOSZOZISJZIZJLSZIOZTOSSZZZOOTJSIOTSJTLOSOLSOLZOTLOLZZTTZJSTISZILITIJJOSZOZJTOSZLSSZSSTZOOZOTSZSSJZISSSSZIOZZTTSSLLOZJSISSOOSJTSISZTOLZJSITZOTZSZSIOJIOIZSTSZLTZZLOLSTTSSJLSOOZJJZTSJZTJZLZZLJOJZZOLZSSZZZOZZSZLJZSSLTZSSSOJIZTZSIJZOSOISZSJZLSJOLZSTSIZZOLOSIZOLLZTTZOOJTITZJJIZJTOOLZZJZZOZSZJSJSOOZLZSZTZIILSZSOJLJTSTZOTIOZTSJZOTZJOZZTSTTJZSTSTSLIZSSILSZZZZOSISOSSOTOJSSLTTIZJIOOLZTZOISOZTJSTOZOZZJZILTJOZSZSSJLZTTLSSOSOZLSIZZZLSZTJLSOJJLTSIOSTZSTTTJOJOZIOTSSZZOOZSOOJSSSOOJSOSTLZOOTOOZSZSITTZZZTTSTSZIZOZLOSSLJOSTILJOSZTSOZOZSTJSSSILOIISSJTOSTLLSZZSSOZLSZOSJSJSSTZJSTLOTZJSSSZJTLLOZOZJSJISJTLIOTOZJSSZZIIJSZJOSJOOOILZIOSLZSIZJSSZZIZZJOLSOLLSZSJSZZIZZSJZOZSZOZZOLOZJJOIZLZSSSTSOIZOZJZZOZLZZOZOSSZZSZOTLTIZSSZLTSLTIZZJOLSZSOISZLSITZOJSZZTSTZTSTLOZOOJSTTOJZTZIOZOTZOTZTTSTZZJTZJOOOSLTSOTTTOSJTZJSJTSZLIZZZTTZOOTZZLTOZSOZTILLTSSSLITTTSJISTJZZJZZLLZTLLTZTZTOLJZZOZLZOSSSLISLTJSJLSTIZTTJTTTTSIZZZOLTLZOSITSOLOTTJSOSZZTZZIZZIILOLZOIZZZSTOZOOLTTSZZZLSITITZSTJZOJILJOSTZOLOOOIJTJOOZLZOOZTSTTISSSLTTIZSLOOJTOSITIZZLIJSSLOOIITSSOZLOJTTIOLSLTJOSZSSTZSOTTSLZILZZZTIJSZZZZZOZSTZZSZJZTSSTOSSSIZSTLSJLTZSTZOJSOSSLZSJSJLSSZTTOZTZSOOTJZZSOTSZTZOJOZJZSSSZZSZTZSSZOSTSSLJIZJSZZTLJSLSZTTITZOLOZOOLOZSZLLSLZSOZZLZOZSSZSOZZJZZOSTSOSSSZSIOTTSTZTOLZSOSJSZOZLSOOISLOOLJOZJZJSZLJTSLLTOSJTZZLTZOTSZZSIZZLZSZOTZOJSOTTSSSSSLTTZOZTZLTSJLOLSJZOZTOOITLOZILSZLOZSSSJTTTZSSZZSSOZSOSLJTOJJTOJZSIOTOZOSZZJZZOSLSJOLTZTOSSOZIZZZTJSLSILOJOSOLZOLJZOZZLZISSSISSTISOILZOJSSTOOOJOJZTLTSLSJTZSSISTSTTZTOJZSSZJOOSLTJTLSSTTZSZZZOZJJJOSSSTSZSTZTZIJTIZZSSOZLTTILSSJOZZSZZTSLOOZSSTTJSSZOIZJOSOJTTLZSZZZOTSLSSJIJTTSZZZSTOTOOSZLZIZZISTSZTJOOIZJSTTZJLOISJSSZZIJSJIOJZOJSZJSLIJTZOTSZSOTLJZIJOZOISTZZSZOIJLSJZTLZSSOSITISSSOOLZSZOSTOZOZLSZOOOZSJJJSLTSISISIOOLJZSZOOZZLSZSOZSZJTZZZIZLZZJJSTSOOSOOZJZLJSTLSTSZSSOTLSOISOJSTTJSSJOSJOOJLSOTIISJJZSISSSOLTIOTJSZZLTIZLSOSZZSTOSJZTJTZJZOZJSOJZLIJSLLSZSJITOSTZTTSZIOOOOLISZTOSZZOJJLOTLOTTOJJTZZSSIJIOSZILZZTZIZOLZLZZSZZSISIJLZIOTOLOSTOZOLJZOOZSOZSJISOZTZJZTIZSTITTSOSLLTLJIZOZZSJJSZZSZLZZOSJOJLOOJOTSZOTLTZZSTOZSZZSTZOZSOISSJLTTLZLJTTLSZZSSJIJJSTSJSIZSZOZLLTSSOTJIZSOOSTZJZJSZJTJOOOTSOLLZLSOTTZJTSOJZOSZOSJLZSIOTOSZLTSILZSJZTOOOZZZJOSJTSTOZOOIOLTTSLSTTSTJOZISTZZLSOLTOZZZTTOIOSTJTTOZSZSSLJZTZOZZTZSOSOOZOLSZZOLJSTSSTOITZTLZSJJIJSTTJSZTOIOOSZSTOJJJOZLJTTTITZOZLISSSSIZTZZTILSZSZZZZSSZTLZTTTTOIJLSTZLOJZOOLZZSOZOOIOJZJZTSOLISLJISZSLLSOSSJJOOLTSZTOOOSZTTZLJTISOTZZTJTLJOZJOOTSZTSIZZZOSISSSJILLZTOOJZZSSZSZOOSSSTOZJZOZZZZSZJSOOZSOOZIZJZTSJSJTJOZOOZTJILIOZZLSSZTOLZIOZZJTSSLITJJLOLTSLLJZSOSTZZJZOOOOISSOLILOJIIIZOJOJOZSTJZTOZJSJOZIOSZSIOZTTLTLZOJOZZTJOZJTJSSJZSIJTZZITZLZJZZZSSOTOOSJIIZTZSSSJSOITLZZSLSSSOJJOTSIOSSZZOTLZSTITZTZTOTTLISIZOZJTTLJJOJSZOISZJZSLLZJSZOOZZSSJOOZLZSSSOOTSZZSZSIZSTSOTLIOSLJZOZTJSOTJSLOJJTOSJSLSLSJSZTOSOJTIOSJLZZZLJJLOJSTOOSZZJIOJSZLSSSOSSSTOZZSJZTZZZSZSLLJOSSLTITJOLZZJZLOZJJOLOZSJZSTZLSIZSOSITOLZZZLZTIJZTOIZZISIOJZZJTSSZSLOSZZZIZSJSOTTIZOOZOLSSIIJLSLIITSSILJZOSSTZLISZZZSJLSLSTZJZSOTSZOJOZTJIZOOZJJSOOOTZLOTTOOSZOOLOSSLOSSZSSTLSZISOOSZIJJZSIILOTZTOSTOZJZJSZOZTOOZOZJOSIZSOOZOJZSSZTOSTTOJZOTSJOLJSOLTLLOZZTSZZZZZTSLZZLOOOZZTTTSOSSSZTOOTIJSJTTZSJZOOLZOTJSZTZLTTSIJZTSOSSZZSOZZZZSLISSJISJJLOZZOJJTSTTZZOSIOLSZLOZOZTLZLZSTSSOTOLSZZSIOOSTJOZSLZJSOSSSOOJSZLOOZJOOOTSSLSTOZTTTJZOJSSLTSIOZZLOILJOIZSZLLZJITOSJJJZLZZOITTTSOZTZSJZSSOJZJLZZOTZITSOLLZSJSLZZZZTTZZZTOJSSIZTTLSTOTISSSZLZOZOLSTZTZZZZOITJLTSTZSIZLLJOJSLOJZOSZOZOSJJZOTZISSSTTSZOZSTJJLTOZSZZOSISSOSZZSSSZTLOZJJOZOSOSZSSOSZTSLSITLTZJOJSOOZSOJOIZIIZLTOJSLTSLOOZSLZZZZSTZZJTOTTSSISOZZOTTZSLJOZZZLSZSJISTSZTZSZZZTSISTTJZLLLZJIJLSJZSZSJLZLLOJTZTZSTTSOLLLZOJIZTTTLZTTZLZTJISOTSSSZJSLJJSSOTLSZZOTTOOLSIIOSJTJZTSSLOZOOZSOOZTOJZTJSJJJOSOLZOSLIIZOSZOTOZLOITSSTJOTJTJSSOSZJTIOSJZSTSZJLIITIZZSSZSSSSSSSOZOSLSOSSZTJZLLOLJISSOJZSZSTSIITSLTSJOZZJSLJZJLSOZOJLZTSJLSIZTJSIZIOJTSOZOLLJZJSSTSSTOTOZZTLJSTZZZZZJZTTTSZOSOTOLZSZZTSIJJTZOZSOOSOISOSZSOOOZSZTZZZSTSTIZTLSZZTOITOSLOJZSJSJTZLSZOSZOIOOZZSTTLSLSZJZJJZSZZTZOZZZOJSTOZSZSZISTSSZSZZTIZOOTZOIOSIOLZOSZILSJOJOLZLLOLOZZJJZOSZLSZJSLSZISZJSOOSSZIJSJIZIOISLIJZLSTLOSZJZOISLTOZLJSZOJLOOJSOSTSOOOIIOOIJZLISZOZSLLSSSZSZSOZZZJTSZOZLTZSZIIZOTTSJTTSOTIJOJOIOSLSJLJSLOZSSOZSOJZIIITJTTSIISOIZLSZJIOOJLTLTZOSISLJZTLSOOOSSJZOZJZOJSJLZIZZSSOOTOZOZSTLOJSLTSZJLSZOSJSSIZSTISTSZLSOSJJSITLJLOJOZTOTSOSTTSISSIIITTSTLOSISSJJZSOTOTOIITTSZSOSLZJOJJZZZIIZZISTSSOISZZSZLOTJJSOSIOJSOZZSTZOJZTIZTIOSZZZTZOZSLLTJOSISZSZTSOZZOSOITISSSZTLTLLSSOZSSSTZTIOZTJZSZOZLTOOOJSIOZZSSSOIOSLTSLOSSOZOSOLTZIZSSJTOTSTOLSLSZZSSIZLSOTZTZLOZZSLSSJSIZZLLOZZSLZJZSLOOIOZZJZZTLTZOJITSSZSJZOTOISLZZSJSSZLZIZOTZOOSSOSSTOSTSJLSLJSOSOTZTTZTZTZSLJJILSZJOZJILZZJLZOJLJTSJISSZOLSISZSZZOTSJLJSSSOISSOILIZSZTJZOSTJTSTSJZTZLTSOTOZJOSSIOSTLZLSIIZSSLLJJOSSZLLSSOZLOOLTOZTILOOJZOZZSJSZZSSJOSTZSOLOSOTOSZSZSZSOOSSLTZOZZJLZJJOOJJOZLZITSSLZTOLSITSZTSLLZJSTOIOOSSZZSIOJZTSIOJSSZIOLTOJJJJZSSLTOJOZZTZSISZZTSZOZZSISTOTTOOTOTZOIZZSZLSJOITIOZSSIZLSJOJZZLLLSSZLLSZSLSTIZLTLLLZZZJOTZOZLTTZOJILLZJZSLITOZZSLZZISOLISOSZOSTLJLSOOOZZJTZTLZZLSJOTZZOIJTZJOLOSIIZSOSLJTJOSSTIZSJZJLTTJSISZZOOOTOLZOTSSLZZLSJOSSSJOTIZIIZZTTSJOSZLOSOZTITZSTZSZZISITJTOJLZOOITZTJOLSOSZISSSSTOOTSOZSISSSSZSOIOSTTJJJSLOZJZOLTSLLOSSIJZJZSLLOTZTIJOSSOOTSSZOJSJZIOJZLOZZOIJZLZSJIZOJTTZLTOSIOIJOZSTLJLZZIZTZZSJISIZZJSOOOLJJTZZZZZZSLJIZSTZZOOJOZSOSSTIZJTSLZSSSJSTZOIOTJZOLLZTTTTTTTT"; // 最后的T都是补位
        public byte[][] Field { get; set; }
        public int Height { get => Field.Length; }
        public int Width { get => Field[0].Length; }
        public int ShowHeight { get; set; }
        public int[] ColHeight { get; set; }
        int CellCount = 0;
        //[NonSerialized]
        public ITetrisRule TetrisRule { get; private set; }
        public int Score { get; set; }
        public Queue<ITetrisMino> NextQueue { get; set; } = new Queue<ITetrisMino>();

        // 对于单方块场地
        public ITetrisMinoStatus TetrisMinoStatus;

        // hold
        public ITetrisMino HoldMino;
        /// <summary>
        /// 生成器 要在这里吗（
        /// </summary>
        public ITetrisMinoGenerator TetrisMinoGenerator;
        //public IFieldCheck FieldCheck => throw new NotImplementedException();

        public GeekTetrisGameBoard(int Width = 10, int Height = 40, int ShowHeight = 20, ITetrisRule tetrisRule = null, ITetrisMinoGenerator tetrisMinoGenerator = null)
        {
            // 赋予规则
            TetrisRule = tetrisRule ?? GeekTetrisRule.Rule;
            TetrisMinoGenerator = tetrisMinoGenerator ?? new GeekGenerator<GeekTetrisMino>();
            Field = new byte[Height][];
            for (int i = 0; i < Height; ++i)
            {
                Field[i] = new byte[Width];
            }
            // ? 也许不用现在取
            this.ShowHeight = Math.Min(ShowHeight, Height);

            //NextQueue.Enqueue(TetrisMinoGenerator.GetNextMino());
            //NextQueue.Enqueue(TetrisMinoGenerator.GetNextMino());
            //NextQueue.Enqueue(TetrisMinoGenerator.GetNextMino());
            //NextQueue.Enqueue(TetrisMinoGenerator.GetNextMino());
            //NextQueue.Enqueue(TetrisMinoGenerator.GetNextMino());
            //NextQueue.Enqueue(TetrisMinoGenerator.GetNextMino());
            SpawnNewPiece();
        }
        //public GeekTetrisGameBoard() { }
        public int TryClearLines()
        {
            int cnt = 0;
            // 限制一下搜索高度
            //List<int> clearidx = new List<int>();
            bool[] clearFlag = new bool[Height];
            for (int i = 0; i < Height; ++i)
            {
                bool flag = true;
                for (int j = 0; j < Width; ++j)
                {
                    if (Field[i][j] == 0)
                    {
                        flag = false;
                    }
                }
                if (flag) { cnt++; clearFlag[i] = true; }
            }
            for (int i = 0, j = 0; i < Height; ++i, ++j)
            {
                while (j < Height && clearFlag[j])
                {
                    ++j;
                }
                if (j >= Height)
                {
                    Field[i] = new byte[Width];
                }
                else
                {
                    Field[i] = Field[j];
                }

            }
            return cnt;
        }
        public int idx { get; set; } = 0;
        
        public void UpdataIdx(int idx)
        {
            this.idx = idx;
            SpawnNewPiece();
        }
        /// <summary>
        /// 输出场地
        /// </summary>
        /// <param name="printLeft"></param>
        /// <param name="printTop"></param>
        public void PrintBoard(bool WithMino = false, int printLeft = 0, int printTop = 0)
        {
            int tempTop = printTop;
            Console.SetCursorPosition(printLeft, printTop); ;
            for (int i = 0; i < Width + 1; ++i) Console.Write("--");
            Console.Write('\n');
            for (int i = 0; i < ShowHeight; ++i)
            {
                Console.SetCursorPosition(printLeft, ++printTop);
                int pi = ShowHeight - 1 - i;
                Console.Write('|');
                for (int j = 0; j < Width; ++j)
                {
                    if (i == 2) { Console.Write("=="); continue; }
                    if (Field[pi][j] != 0)
                    {

                        //Console.Write("[]");
                        Console.Write("■");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.Write('|');
                Console.Write('\n');
            }
            Console.SetCursorPosition(printLeft, ++printTop);
            for (int i = 0; i < Width + 1; ++i) Console.Write("--");
            Console.Write('\n');
            foreach (var m in gg[(idx + 1)..(idx + 7)])
            {
                Console.Write(m);
            }
            Console.WriteLine($"\n{idx} pieces; {Score} score");
            
            //Console.ForegroundColor = ConsoleColor.Yellow;
            if (IsDead) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("i think you died   ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else Console.WriteLine("you alive now   ");
            Console.WriteLine($"Next I will appear after {gg.IndexOf("I", idx + 1) - idx} pieces");
            Console.WriteLine($"Next O will appear after {gg.IndexOf("O", idx + 1) - idx} pieces");
            Console.WriteLine($"Next J will appear after {gg.IndexOf("J", idx + 1) - idx} pieces");
            Console.WriteLine($"Next L will appear after {gg.IndexOf("L", idx + 1) - idx} pieces");
            Console.WriteLine($"Next T will appear after {gg.IndexOf("T", idx + 1) - idx} pieces");
            Console.WriteLine($"Next Z will appear after {gg.IndexOf("Z", idx + 1) - idx} pieces");
            Console.WriteLine($"Next S will appear after {gg.IndexOf("S", idx + 1) - idx} pieces");
            //Console.WriteLine(res.ToString());
            ITetrisMinoStatus ghost = new TetrisMinoStatus { Position = TetrisMinoStatus.Position, Stage = TetrisMinoStatus.Stage, TetrisMino = TetrisMinoStatus.TetrisMino };
            while (true) {
                ghost.MoveBottom();
                if (!TetrisRule.CheckMinoOk(this, ghost))
                {
                    break;
                }
            }
            ghost.MoveTop();
            if (WithMino && TetrisMinoStatus != null)
            {

                foreach (var pos in ghost?.GetMinoFieldListInBoard())
                {
                    // pos 要在显示区域内
                    // 肯定有问题.jpg
                    Console.SetCursorPosition(printLeft + 1 + pos.Y * 2, tempTop + (ShowHeight - pos.X));
                    Console.Write("{}");
                }
                foreach (var pos in TetrisMinoStatus?.GetMinoFieldListInBoard())
                {
                    // pos 要在显示区域内
                    // 肯定有问题.jpg
                    Console.SetCursorPosition(printLeft + 1 + pos.Y * 2, tempTop + (ShowHeight - pos.X));
                    Console.Write("■");
                    //Console.Write("[]");
                }
            }
        }

        LinkedList<(byte[][] field, int score)> prevres = new LinkedList<(byte[][], int score)>();

        byte[][] CopyByteArray()
        {
            byte[][] prevField = new byte[Height][];
            for (int i = 0; i < Height; ++i)
            {
                prevField[i] = (byte[])Field[i].Clone();
            }
            return prevField;
        }
        public bool IsDead => !TetrisRule.CheckMinoOk(this, TetrisMinoStatus) || Field[19].Any(s => s != 0);
        public bool LockMino()
        {
            prevres.AddLast((CopyByteArray(), Score));
            if (prevres.Count > 1000) prevres.RemoveFirst();
            //if(TetrisRule)
            idx++;
            var minoList = TetrisMinoStatus.GetMinoFieldListInBoard();
            // 要不不检查了（？
            // 断言此时的场地和方块是ok的
            foreach (var pos in minoList)
            {
                Field[pos.X][pos.Y] = 1;
            }
            CellCount += 4;
            SpawnNewPiece();

            int linecnt = TryClearLines();

            Score += linecnt switch
            {
                1 => CellCount,
                2 => 3 * CellCount,
                3 => 6 * CellCount,
                4 => 10 * CellCount,
                _ => 0,
            };
            CellCount -= linecnt * Width;
            return true;
        }



        public bool IsCellFree(int x, int y)
        {
            if (x >= 0 && x < Height && y >= 0 && y < Width)
            {
                return Field[x][y] == 0;
            }
            return false;
        }

        public bool LeftRotation()
        {
            return TetrisRule.RotationSystem.LeftRotation(this, TetrisMinoStatus).isSuccess;
        }

        public bool RightRotation()
        {
            if (TetrisRule.RotationSystem.RightRotation(this, TetrisMinoStatus).isSuccess)
            {
                res.Append('C');
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool _180Rotation()
        {
            throw new NotImplementedException();
        }

        public bool MoveLeft()
        {
            TetrisMinoStatus.MoveLeft();
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {
                res.Append('L');
                return true;
            }
            TetrisMinoStatus.MoveRight();
            return false;
        }

        public bool MoveRight()
        {
            TetrisMinoStatus.MoveRight();
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {
                res.Append('R');
                return true;
            }
            TetrisMinoStatus.MoveLeft();
            return false;
        }

        public bool SoftDrop()
        {
            TetrisMinoStatus.MoveBottom();
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {
                res.Append('D');
                return true;
            }
            TetrisMinoStatus.MoveTop();
            return false;
        }

        public bool SonicDrop()
        {
            while (SoftDrop()) ;
            return true;
        }
        /// <summary>
        /// 硬降 需要确认方块位置没问题吗
        /// </summary>
        /// <returns></returns>
        public bool HardDrop()
        {

            SonicDrop();
            LockMino();
            return true;
        }

        public bool OnHold()
        {
            // if 允许hold

            if (HoldMino == null)
            {
                HoldMino = TetrisMinoStatus.TetrisMino;
                SpawnNewPiece();
            }
            else
            {
                (HoldMino, TetrisMinoStatus.TetrisMino) = (TetrisMinoStatus.TetrisMino, HoldMino);
                TetrisMinoStatus.Position = (19, 3);
                TetrisMinoStatus.Stage = 0;
            }
            return true;
        }
        public int stageidx { get; set; } = 0;

        GeekTetrisMino mb = new GeekTetrisMino();
        public bool SpawnNewPiece()
        {
            //NextQueue.Enqueue(TetrisMinoGenerator.GetNextMino());

            ITetrisMino[] list = mb.GetMinoList();
            // 先简略来一个（ 后续要改 要考虑方块用什么 需不需要接口 要看看成不成功
            //TetrisMinoStatus = new TetrisMinoStatus { Position = (19, 4), Stage = (stageidx++)%4, TetrisMino = TetrisMinoGenerator.GetNextMino() };
            //TetrisMinoStatus = new TetrisMinoStatus { Position = (19, 4), Stage = (stageidx++)%4, TetrisMino = NextQueue.Dequeue() };
            TetrisMinoStatus = new TetrisMinoStatus { Position = (19, 4), Stage = (stageidx++) % 4, TetrisMino = list.First(s => s.Name[0] == gg[idx]) };
            
            res.Append('N');
            return true;
        }



        public bool ReturnPrev()
        {
            // 如果没有前面的场地则无法回溯
            if (prevres.Count > 0)
            {
                Field = prevres.Last().field;
                Score = prevres.Last().score;
                prevres.RemoveLast();
            }
            else
            {
                return false;
            }
            int residx = res.Length - 1;
            while (res[residx] != 'N')
            {
                residx--;
            }
            residx--;
            while (res[residx] != 'N')
            {
                residx--;
            }
            CellCount = 0;
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    if (Field[i][j] != 0)
                    {
                        CellCount++;
                    }
                }
            }
            res.Remove(residx, res.Length - residx);
            idx--;
            stageidx-=2;
            stageidx = (stageidx + 4) % 4;
            SpawnNewPiece();
            return true;
        }

        public bool IsCellFreeWithMino(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void ResetGame()
        {
            throw new NotImplementedException();
        }

        public void ReceiveGarbage(List<int> garbages)
        {
            throw new NotImplementedException();
        }

        public bool MoveUp()
        {
            throw new NotImplementedException();
        }

        public StringBuilder res { get; set; } = new StringBuilder();

        public string GetRes  { get => res.ToString()[1..]; set => res = new StringBuilder("N" + value); }

    }
}
