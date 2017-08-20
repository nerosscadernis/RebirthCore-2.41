using Rebirth.World.Datas.Interactives.Types;
using Rebirth.World.Datas.Interactives.Types.Classic;
using Rebirth.World.Datas.Maps;
using Rebirth.World.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebirth.World.Datas.Interactives.Creation
{
    public class InteractiveCreator
    {
        #region Zaaps
        [Interactive("34681")]
        private void HandleZaapPorteAstrub(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("38003")]
        private void HandleZaapAstrub(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("37410")]
        private void HandleZaapSufokia(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("15362")]
        private void HandleZaapChateauAmakna(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("15363")]
        private void HandleZaapKrakleur(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("21830")]
        private void HandleZaapFrigost(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("16971")]
        private void HandleZaapPandala(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("19804")]
        private void HandleZaapCanope(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("17268")]
        private void HandleZaapBonta(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("16982")]
        private void HandleZaapBrakmar(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("24193")]
        private void HandleZaapKoalak(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("30091")]
        private void HandleZaapMasterdam(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("39045")]
        private void HandleZaapWabbit(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        [Interactive("41724")]
        private void HandleZaapAlliance(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaap(identifier, cellid));
        }
        #endregion

        #region Zaapis
        [Interactive("9541")]
        private void HandleZaapiBonta(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            var interactive = new InteractiveZaapi(identifier, cellid, subArea, map.Id, onMap);
            map.AddInteractive(interactive);
            if (!onMap)
                MapManager.Instance.AddZaapi(interactive as InteractiveZaapi);
        }
        [Interactive("15004")]
        private void HandleZaapiBrak(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveZaapi(identifier, cellid, subArea, map.Id, onMap));
        }
        #endregion
        
        #region Couture
        [Interactive("9797")]
        private void HandleAtelierCouture1(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 86, new uint[] { 63 }, map.Id));
        }
        [Interactive("9798")]
        private void HandleAtelierCouture2(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 86, new uint[] { 63 }, map.Id));
        }
        #endregion

        #region Sculteur
        [Interactive("9796")]
        private void HandleAtelierSculteur(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 13, new uint[] { 15 }, map.Id));
        }
        #endregion

        #region Coordonnier
        [Interactive("38188")]
        private void HandleAtelierCoordonnier(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 11, new uint[] { 13 }, map.Id));
        }
        #endregion

        #region Bricoleur
        [Interactive("49504")]
        private void HandleAtelierBricoleur(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 122, new uint[] { 171 }, map.Id));
        }
        [Interactive("14604")]
        private void HandleAtelierBricoleur1(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 122, new uint[] { 171 }, map.Id));
        }
        #endregion

        #region Boucher
        [Interactive("14601")]
        private void HandleAtelierBoucher(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 97, new uint[] { 134 }, map.Id));
        }
        #endregion

        #region Idole - Bouclier - Trophee
        [Interactive("49574")]
        private void HandleAtelierIdole(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 12, new uint[] { 297 }, map.Id));
        }
        [Interactive("49573")]
        private void HandleAtelierIdole1(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 12, new uint[] { 297 }, map.Id));
        }
        [Interactive("54087")]
        private void HandleAtelierIdole2(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 12, new uint[] { 297 }, map.Id));
        }
        [Interactive("49503")]
        private void HandleAtelierBouclier(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 138, new uint[] { 201 }, map.Id));
        }
        [Interactive("32494")]
        private void HandleAtelierTrophe(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveAtelier(identifier, cellid, 97, new uint[] { 156 }, map.Id));
        }
        #endregion

        #region Ressources
        [Interactive("{224}")]
        private void HandlePuit(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 84, new uint[] { 102 }, map.Id, onMap));
        }
        [Interactive("{650}")]
        private void HandleFrene(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 1, new uint[] { 6 }, map.Id, onMap));
        }
        [Interactive("{3715}")]
        private void HandleFreneBis(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 1, new uint[] { 6 }, map.Id, onMap));
        }
        [Interactive("{651}")]
        private void HandleChateigner(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 33, new uint[] { 39 }, map.Id, onMap));
        }
        [Interactive("{652}")]
        private void HandleNoyer(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 34, new uint[] { 40 }, map.Id, onMap));
        }
        [Interactive("{653}")]
        private void HandleChene(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 8, new uint[] { 10 }, map.Id, onMap));
        }
        [Interactive("{654}")]
        private void HandleErable(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 31, new uint[] { 37 }, map.Id, onMap));
        }
        [Interactive("{655}")]
        private void HandleIf(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 28, new uint[] { 33 }, map.Id, onMap));
        }
        [Interactive("{656}")]
        private void HandleMerisier(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 35, new uint[] { 41 }, map.Id, onMap));
        }
        [Interactive("{657}")]
        private void HandleEbene(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 29, new uint[] { 34 }, map.Id, onMap));
        }
        [Interactive("{658}")]
        private void HandleCharme(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 32, new uint[] { 38 }, map.Id, onMap));
        }
        [Interactive("{659}")]
        private void HandleOrme(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 30, new uint[] { 35 }, map.Id, onMap));
        }
        [Interactive("{660}")]
        private void HandleBle(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 38, new uint[] { 45 }, map.Id, onMap));
        }
        [Interactive("{661}")]
        private void HandleHoublon(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 39, new uint[] { 46 }, map.Id, onMap));
        }
        [Interactive("{662}")]
        private void HandleLin(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 42, new uint[] { 50 }, map.Id, onMap));
        }
        [Interactive("{663}")]
        private void HandleChanvre(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 46, new uint[] { 54 }, map.Id, onMap));
        }
        [Interactive("{664}")]
        private void HandleOrge(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 43, new uint[] { 53 }, map.Id, onMap));
        }
        [Interactive("{665}")]
        private void HandleSeigle(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 44, new uint[] { 52 }, map.Id, onMap));
        }
        [Interactive("{667}")]
        private void HandleMalt(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 47, new uint[] { 58 }, map.Id, onMap));
        }
        [Interactive("{677}")]
        private void HandleTrefle(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 67, new uint[] { 71 }, map.Id, onMap));
        }
        [Interactive("{678}")]
        private void HandleMenthe(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 66, new uint[] { 72 }, map.Id, onMap));
        }
        [Interactive("{679}")]
        private void HandleOrchide(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 68, new uint[] { 73 }, map.Id, onMap));
        }
        [Interactive("{680}")]
        private void HandleEdeweiss(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 61, new uint[] { 74 }, map.Id, onMap));
        }
        [Interactive("{681}")]
        private void HandleBombu(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 98, new uint[] { 139 }, map.Id, onMap));
        }
        [Interactive("{682}")]
        private void HandleOliviolet(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 101, new uint[] { 141 }, map.Id, onMap));
        }
        [Interactive("{683}")]
        private void HandleRiz(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 111, new uint[] { 159 }, map.Id, onMap));
        }
        [Interactive("{684}")]
        private void HandlePandouille(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 112, new uint[] { 160 }, map.Id, onMap));
        }
        [Interactive("{685}")]
        private void HandleBambou(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 108, new uint[] { 154 }, map.Id, onMap));
        }
        [Interactive("{686}")]
        private void HandleBambouSombre(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 109, new uint[] { 155 }, map.Id, onMap));
        }
        [Interactive("{689}")]
        private void HandleKaliptus(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 121, new uint[] { 174 }, map.Id, onMap));
        }
        [Interactive("{701}")]
        private void HandleAvoine(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 45, new uint[] { 57 }, map.Id, onMap));
        }
        [Interactive("{1018}")]
        private void HandleGoujon(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 75, new uint[] { 124 }, map.Id, onMap));
        }
        [Interactive("{1019}")]
        private void HandleTruite(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 74, new uint[] { 125 }, map.Id, onMap));
        }
        [Interactive("{1029}")]
        private void HandleBambouSacree(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 259, new uint[] { 306 }, map.Id, onMap));
        }
        [Interactive("{1074}")]
        private void HandleBronze(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 55, new uint[] { 26 }, map.Id, onMap));
        }
        [Interactive("{1075}")]
        private void HandleCuivre(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 53, new uint[] { 25 }, map.Id, onMap));
        }
        [Interactive("{1077}")]
        private void HandleEtain(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 52, new uint[] { 55 }, map.Id, onMap));
        }
        [Interactive("{1081}")]
        private void HandleFer(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 17, new uint[] { 24 }, map.Id, onMap));
        }
        [Interactive("{2204}")]
        private void HandleFerBis(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 17, new uint[] { 24 }, map.Id, onMap));
        }
        [Interactive("{1245}")]
        private void HandleFrostiz(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 134, new uint[] { 191 }, map.Id, onMap));
        }
        [Interactive("{1288}")]
        private void HandlePerceNeige(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 131, new uint[] { 188 }, map.Id, onMap));
        }
        [Interactive("{1289}")]
        private void HandleTremble(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 133, new uint[] { 190 }, map.Id, onMap));
        }
        [Interactive("{3212}")]
        private void HandleOrtie(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 254, new uint[] { 68 }, map.Id, onMap));
        }
        [Interactive("{3213}")]
        private void HandleSauge(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 255, new uint[] { 69 }, map.Id, onMap));
        }
        [Interactive("{3222}")]
        private void HandleNoisetier(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 259, new uint[] { 306 }, map.Id, onMap));
        }
        [Interactive("{3223}")]
        private void HandleBelladone(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 257, new uint[] { 304 }, map.Id, onMap));
        }
        [Interactive("{3226}")]
        private void HandleMandragor(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 258, new uint[] { 305 }, map.Id, onMap));
        }
        [Interactive("{3228}")]
        private void HandleMais(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 260, new uint[] { 307 }, map.Id, onMap));
        }
        [Interactive("{3230}")]
        private void HandleMillet(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 261, new uint[] { 308 }, map.Id, onMap));
        }
        [Interactive("{3234}")]
        private void HandleGinseng(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 256, new uint[] { 303 }, map.Id, onMap));
        }
        [Interactive("{1063}")]
        private void HandleKobalt(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 37, new uint[] { 28 }, map.Id, onMap));
        }
        [Interactive("{1078}")]
        private void HandleManganese(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 54, new uint[] { 56 }, map.Id, onMap));
        }
        [Interactive("{1073}")]
        private void HandleBauxite(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 26, new uint[] { 31 }, map.Id, onMap));
        }
        [Interactive("{1076}")]
        private void HandleSillicate(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 114, new uint[] { 162 }, map.Id, onMap));
        }
        [Interactive("{3553}")]
        private void HandleEcu(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new Ressource(identifier, cellid, 291, new uint[] { 342 }, map.Id, onMap));
        }
        [Interactive("{3518}")]
        private void HandleJeNeSaisPlus(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveTrigger(identifier, cellid));
        }
        [Interactive("{3507}")]
        private void HandleJeSaisPas(MapTemplate map, int identifier, uint cellid, bool onMap, int subArea)
        {
            map.AddInteractive(new InteractiveTrigger(identifier, cellid));
        }
        #endregion
    }
}
