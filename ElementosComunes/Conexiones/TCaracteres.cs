
namespace ElementosComunes.Conexiones
{
    public class TCaracteres
    {
        public static string CodificarCaracteres(string CADENA)
        {
            CADENA = CADENA.Replace("☺", "&1;");
            CADENA = CADENA.Replace("☻", "&2;");
            CADENA = CADENA.Replace("♥", "&3;");
            CADENA = CADENA.Replace("♦", "&4;");
            CADENA = CADENA.Replace("♣", "&5;");
            CADENA = CADENA.Replace("♠", "&6;");
            CADENA = CADENA.Replace("•", "&7;");
            CADENA = CADENA.Replace("◘", "&8;");
            CADENA = CADENA.Replace("○", "&9;");
            CADENA = CADENA.Replace("◙", "&10;");
            CADENA = CADENA.Replace("♂", "&11;");
            CADENA = CADENA.Replace("♀", "&12;");
            CADENA = CADENA.Replace("♪", "&13;");
            CADENA = CADENA.Replace("♫", "&14;");
            CADENA = CADENA.Replace("☼", "&15;");
            CADENA = CADENA.Replace("►", "&16;");
            CADENA = CADENA.Replace("◄", "&17;");
            CADENA = CADENA.Replace("↕", "&18;");
            CADENA = CADENA.Replace("‼", "&19;");
            CADENA = CADENA.Replace("¶", "&20;");
            CADENA = CADENA.Replace("§", "&21;");
            CADENA = CADENA.Replace("▬", "&22;");
            CADENA = CADENA.Replace("↨", "&23;");
            CADENA = CADENA.Replace("↑", "&24;");
            CADENA = CADENA.Replace("↓", "&25;");
            CADENA = CADENA.Replace("→", "&26;");
            CADENA = CADENA.Replace("←", "&27;");
            CADENA = CADENA.Replace("∟", "&28;");
            CADENA = CADENA.Replace("↔", "&29;");
            CADENA = CADENA.Replace("▲", "&30;");
            CADENA = CADENA.Replace("▼", "&31;");
            CADENA = CADENA.Replace(" ", "&32;");
            CADENA = CADENA.Replace("!", "&33;");
            CADENA = CADENA.Replace("'", "&34;");
            CADENA = CADENA.Replace("#", "&35;");
            CADENA = CADENA.Replace("$", "&36;");
            CADENA = CADENA.Replace("%", "&37;");
            //CADENA = CADENA.Replace("&", "&38;");
            CADENA = CADENA.Replace("'", "&39;");
            CADENA = CADENA.Replace("(", "&40;");
            CADENA = CADENA.Replace(")", "&41;");
            CADENA = CADENA.Replace("*", "&42;");
            CADENA = CADENA.Replace("+", "&43;");
            CADENA = CADENA.Replace(",", "&44;");
            CADENA = CADENA.Replace("-", "&45;");
            CADENA = CADENA.Replace(".", "&46;");
            CADENA = CADENA.Replace("/", "&47;");
            /*CADENA = CADENA.Replace("0", "&48;",);
            CADENA = CADENA.Replace("1", "&49;");
            CADENA = CADENA.Replace("2", "&50;");
            CADENA = CADENA.Replace("3", "&51;",);
            CADENA = CADENA.Replace("4", "&52;");
            CADENA = CADENA.Replace("5", "&53;");
            CADENA = CADENA.Replace("6", "&54;");
            CADENA = CADENA.Replace("7", "&55;");
            CADENA = CADENA.Replace("8", "&56;");
            CADENA = CADENA.Replace("9", "&57;");*/
            CADENA = CADENA.Replace(":", "&58;");
            //CADENA = CADENA.Replace(";", "&59;");
            CADENA = CADENA.Replace("<", "&60;");
            CADENA = CADENA.Replace("=", "&61;");
            CADENA = CADENA.Replace(">", "&62;");
            CADENA = CADENA.Replace("?", "&63;");
            CADENA = CADENA.Replace("@", "&64;");
            /*CADENA = CADENA.Replace("A", "&65;");
            CADENA = CADENA.Replace("B", "&66;");
            CADENA = CADENA.Replace("C", "&67;");
            CADENA = CADENA.Replace("D", "&68;");
            CADENA = CADENA.Replace("E", "&69;");
            CADENA = CADENA.Replace("F", "&70;");
            CADENA = CADENA.Replace("G", "&71;");
            CADENA = CADENA.Replace("H", "&72;");
            CADENA = CADENA.Replace("I", "&73;");
            CADENA = CADENA.Replace("J", "&74;");
            CADENA = CADENA.Replace("K", "&75;");
            CADENA = CADENA.Replace("L", "&76;");
            CADENA = CADENA.Replace("M", "&77;");
            CADENA = CADENA.Replace("N", "&78;");
            CADENA = CADENA.Replace("O", "&79;");
            CADENA = CADENA.Replace("P", "&80;");
            CADENA = CADENA.Replace("Q", "&81;");
            CADENA = CADENA.Replace("R", "&82;");
            CADENA = CADENA.Replace("S", "&83;");
            CADENA = CADENA.Replace("T", "&84;");
            CADENA = CADENA.Replace("U", "&85;");
            CADENA = CADENA.Replace("V", "&86;");
            CADENA = CADENA.Replace("W", "&87;");
            CADENA = CADENA.Replace("X", "&88;");
            CADENA = CADENA.Replace("Y", "&89;");
            CADENA = CADENA.Replace("Z", "&90;");*/
            CADENA = CADENA.Replace("[", "&91;");
            CADENA = CADENA.Replace("\\", "&92;");
            CADENA = CADENA.Replace("]", "&93;");
            CADENA = CADENA.Replace("^", "&94;");
            CADENA = CADENA.Replace("_", "&95;");
            CADENA = CADENA.Replace("`", "&96;");
            /*CADENA = CADENA.Replace("a", "&97;");
            CADENA = CADENA.Replace("b", "&98;");
            CADENA = CADENA.Replace("c", "&99;");
            CADENA = CADENA.Replace("d", "&100;");
            CADENA = CADENA.Replace("e", "&101;");
            CADENA = CADENA.Replace("f", "&102;");
            CADENA = CADENA.Replace("g", "&103;");
            CADENA = CADENA.Replace("h", "&104;");
            CADENA = CADENA.Replace("i", "&105;");
            CADENA = CADENA.Replace("j", "&106;");
            CADENA = CADENA.Replace("k", "&107;");
            CADENA = CADENA.Replace("l", "&108;");
            CADENA = CADENA.Replace("m", "&109;");
            CADENA = CADENA.Replace("n", "&110;");
            CADENA = CADENA.Replace("o", "&111;");
            CADENA = CADENA.Replace("p", "&112;");
            CADENA = CADENA.Replace("q", "&113;");
            CADENA = CADENA.Replace("r", "&114;");
            CADENA = CADENA.Replace("s", "&115;");
            CADENA = CADENA.Replace("t", "&116;");
            CADENA = CADENA.Replace("u", "&117;");
            CADENA = CADENA.Replace("v", "&118;");
            CADENA = CADENA.Replace("w", "&119;");
            CADENA = CADENA.Replace("x", "&120;");
            CADENA = CADENA.Replace("y", "&121;");
            CADENA = CADENA.Replace("z", "&122;");*/
            CADENA = CADENA.Replace("{", "&123;");
            CADENA = CADENA.Replace("|", "&124;");
            CADENA = CADENA.Replace("}", "&125;");
            CADENA = CADENA.Replace("~", "&126;");
            CADENA = CADENA.Replace("⌂", "&127;");
            CADENA = CADENA.Replace("Ç", "&128;");
            CADENA = CADENA.Replace("ü", "&129;");
            CADENA = CADENA.Replace("é", "&130;");
            CADENA = CADENA.Replace("â", "&131;");
            CADENA = CADENA.Replace("ä", "&132;");
            CADENA = CADENA.Replace("à", "&133;");
            CADENA = CADENA.Replace("å", "&134;");
            CADENA = CADENA.Replace("ç", "&135;");
            CADENA = CADENA.Replace("ê", "&136;");
            CADENA = CADENA.Replace("ë", "&137;");
            CADENA = CADENA.Replace("è", "&138;");
            CADENA = CADENA.Replace("ï", "&139;");
            CADENA = CADENA.Replace("î", "&140;");
            CADENA = CADENA.Replace("ì", "&141;");
            CADENA = CADENA.Replace("Ä", "&142;");
            CADENA = CADENA.Replace("Å", "&143;");
            CADENA = CADENA.Replace("É", "&144;");
            CADENA = CADENA.Replace("æ", "&145;");
            CADENA = CADENA.Replace("Æ", "&146;");
            CADENA = CADENA.Replace("ô", "&147;");
            CADENA = CADENA.Replace("ö", "&148;");
            CADENA = CADENA.Replace("ò", "&149;");
            CADENA = CADENA.Replace("û", "&150;");
            CADENA = CADENA.Replace("ù", "&151;");
            CADENA = CADENA.Replace("ÿ", "&152;");
            CADENA = CADENA.Replace("Ö", "&153;");
            CADENA = CADENA.Replace("Ü", "&154;");
            CADENA = CADENA.Replace("ø", "&155;");
            CADENA = CADENA.Replace("£", "&156;");
            CADENA = CADENA.Replace("Ø", "&157;");
            CADENA = CADENA.Replace("×", "&158;");
            CADENA = CADENA.Replace("ƒ", "&159;");
            CADENA = CADENA.Replace("á", "&160;");
            CADENA = CADENA.Replace("í", "&161;");
            CADENA = CADENA.Replace("ó", "&162;");
            CADENA = CADENA.Replace("ú", "&163;");
            CADENA = CADENA.Replace("ñ", "&164;");
            CADENA = CADENA.Replace("Ñ", "&165;");
            CADENA = CADENA.Replace("ª", "&166;");
            CADENA = CADENA.Replace("º", "&167;");
            CADENA = CADENA.Replace("¿", "&168;");
            CADENA = CADENA.Replace("®", "&169;");
            CADENA = CADENA.Replace("¬", "&170;");
            CADENA = CADENA.Replace("½", "&171;");
            CADENA = CADENA.Replace("¼", "&172;");
            CADENA = CADENA.Replace("¡", "&173;");
            CADENA = CADENA.Replace("«", "&174;");
            CADENA = CADENA.Replace("»", "&175;");
            CADENA = CADENA.Replace("░", "&176;");
            CADENA = CADENA.Replace("▒", "&177;");
            CADENA = CADENA.Replace("▓", "&178;");
            CADENA = CADENA.Replace("│", "&179;");
            CADENA = CADENA.Replace("┤", "&180;");
            CADENA = CADENA.Replace("Á", "&181;");
            CADENA = CADENA.Replace("Â", "&182;");
            CADENA = CADENA.Replace("À", "&183;");
            CADENA = CADENA.Replace("©", "&184;");
            CADENA = CADENA.Replace("╣", "&185;");
            CADENA = CADENA.Replace("║", "&186;");
            CADENA = CADENA.Replace("╗", "&187;");
            CADENA = CADENA.Replace("╝", "&188;");
            CADENA = CADENA.Replace("¢", "&189;");
            CADENA = CADENA.Replace("¥", "&190;");
            CADENA = CADENA.Replace("┐", "&191;");
            CADENA = CADENA.Replace("└", "&192;");
            CADENA = CADENA.Replace("┴", "&193;");
            CADENA = CADENA.Replace("┬", "&194;");
            CADENA = CADENA.Replace("├", "&195;");
            CADENA = CADENA.Replace("─", "&196;");
            CADENA = CADENA.Replace("┬", "&197;");
            CADENA = CADENA.Replace("ã", "&198;");
            CADENA = CADENA.Replace("Ã", "&199;");
            CADENA = CADENA.Replace("╚", "&200;");
            CADENA = CADENA.Replace("╔", "&201;");
            CADENA = CADENA.Replace("╩", "&202;");
            CADENA = CADENA.Replace("╦", "&203;");
            CADENA = CADENA.Replace("╠", "&204;");
            CADENA = CADENA.Replace("═", "&205;");
            CADENA = CADENA.Replace("╬", "&206;");
            CADENA = CADENA.Replace("¤", "&207;");
            CADENA = CADENA.Replace("ð", "&208;");
            CADENA = CADENA.Replace("Ð", "&209;");
            CADENA = CADENA.Replace("Ê", "&210;");
            CADENA = CADENA.Replace("Ë", "&211;");
            CADENA = CADENA.Replace("È", "&212;");
            CADENA = CADENA.Replace("ı", "&213;");
            CADENA = CADENA.Replace("Í", "&214;");
            CADENA = CADENA.Replace("Î", "&215;");
            CADENA = CADENA.Replace("Ï", "&216;");
            CADENA = CADENA.Replace("┘", "&217;");
            CADENA = CADENA.Replace("┌", "&218;");
            CADENA = CADENA.Replace("█", "&219;");
            CADENA = CADENA.Replace("▄", "&220;");
            CADENA = CADENA.Replace("¦", "&221;");
            CADENA = CADENA.Replace("Ì", "&222;");
            CADENA = CADENA.Replace("▀", "&223;");
            CADENA = CADENA.Replace("Ó", "&224;");
            CADENA = CADENA.Replace("ß", "&225;");
            CADENA = CADENA.Replace("Ô", "&226;");
            CADENA = CADENA.Replace("Ò", "&227;");
            CADENA = CADENA.Replace("õ", "&228;");
            CADENA = CADENA.Replace("Õ", "&229;");
            CADENA = CADENA.Replace("µ", "&230;");
            CADENA = CADENA.Replace("þ", "&231;");
            CADENA = CADENA.Replace("Þ", "&232;");
            CADENA = CADENA.Replace("Ú", "&233;");
            CADENA = CADENA.Replace("Û", "&234;");
            CADENA = CADENA.Replace("Ù", "&235;");
            CADENA = CADENA.Replace("ý", "&236;");
            CADENA = CADENA.Replace("Ý", "&237;");
            CADENA = CADENA.Replace("¯", "&238;");
            CADENA = CADENA.Replace("´", "&239;");
            CADENA = CADENA.Replace("­", "&240;");
            CADENA = CADENA.Replace("±", "&241;");
            CADENA = CADENA.Replace("‗", "&242;");
            CADENA = CADENA.Replace("¾", "&243;");
            CADENA = CADENA.Replace("¶", "&244;");
            CADENA = CADENA.Replace("§", "&245;");
            CADENA = CADENA.Replace("÷", "&246;");
            CADENA = CADENA.Replace("¸", "&247;");
            CADENA = CADENA.Replace("°", "&248;");
            CADENA = CADENA.Replace("¨", "&249;");
            CADENA = CADENA.Replace("·", "&250;");
            CADENA = CADENA.Replace("¹", "&251;");
            CADENA = CADENA.Replace("³", "&252;");
            CADENA = CADENA.Replace("²", "&253;");
            CADENA = CADENA.Replace("■", "&254;");
            CADENA = CADENA.Replace(" ", "&255;");
            CADENA = CADENA.Replace("\\n", "&256;");
            CADENA = CADENA.Replace("\n", "&256;");
            CADENA = CADENA.Replace("\\r", "&256;");

            return CADENA;
        }

        public static string RemplazarCaracteres(string JSON)
        {
            string respuesta = "";
            if (!string.IsNullOrEmpty(JSON))
            {
                // Remplaza las comillas dobles por simples en un texto
                respuesta = JSON.Replace("\\\"", "'");
                // Remplaza las diagonal invertida por diagonal simplre en un texto
                respuesta = respuesta.Replace("\\\\", "/");
                respuesta = respuesta.Replace("\\", "/");
            }
            else
                respuesta = JSON;
            return respuesta;
        }

        public static string CodificarMensajeWeb(string CADENA)
        {
            string respuesta = "";
            if (!string.IsNullOrEmpty(CADENA))
            {                
                respuesta = RemplazarCaracteres(CADENA.Trim());
                respuesta = CodificarCaracteres(respuesta);
            }
            return respuesta;
        }

        public static string QuitarAcentos(string TEXTO)
        {
            string respuesta = "";
            if (!string.IsNullOrEmpty(TEXTO))
            {
                respuesta = TEXTO;
                respuesta = respuesta.Replace('Á', 'A');
                respuesta = respuesta.Replace('É', 'E');
                respuesta = respuesta.Replace('Í', 'I');
                respuesta = respuesta.Replace('Ó', 'O');
                respuesta = respuesta.Replace('Ú', 'U');
                respuesta = respuesta.Replace('Ñ', 'N');
                respuesta = respuesta.Replace('á', 'a');
                respuesta = respuesta.Replace('é', 'e');
                respuesta = respuesta.Replace('í', 'i');
                respuesta = respuesta.Replace('ó', 'o');
                respuesta = respuesta.Replace('ú', 'u');
                respuesta = respuesta.Replace('ñ', 'n');
            }
            return respuesta;
        }
    }
}
