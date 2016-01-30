# VEZIR
VEZetői Információs Rendszer

## DataBase
1. szükséges adatbázisok backup-jai 
	* vezir_alap...      a rendszer kivételével üresek Ezeket rendre vezir_rendszer, vezir_user, vezir_alkalm1 néven célszerü  visszamenteni (föként az eredeti neveket ne használd) 
2. hülye tesztadatokkal az adatbázisok a feeling kedvéért
	* Bejelentkezés: Gitta (a jelszó is)

A TERVEZO és a Vezir bin\Debug  ill bin\Release folder- ben is van egy-egy Connection.Txt,  ott kell a ConnectionString-eket javitani. 

## Kezelői szerep
alkalmazásonként, azon belül cégenként értelmezett

1. Ha az alkalmazáshoz egyetlen kezelőt rendeltünk, az lesz az alkalmazás rend- szergazdája és az egy vagy több cég kizárólagos kezelője. Természetesen mindenhez teljes (irás+olvasás) hozzáférése van.
2.  Ha az alkalmazáshoz több kezelőt rendeltünk, valamelyik rendszergazda lesz, és cégenként kell megadni egy-egy kezelő szerepét az adott cégnél, mely lehet:
	* Kiemelt kezelő
	* Kezelő
	* Kiemelt kezelő+Kezelő
	* Vezető
	* Semmi

## Mit tehet egy kezelő?

#### Rendszergazda 
* irás+olvasás hozzáférése van az alkalmazás beállitásaihoz, cégenkénti beállitásadatokhoz, változásnapló- és logtáblaadatokhoz.
* Más adathoz nincs hozzáférése.

#### Kiemelt kezelő
* hozzáférése van cégszinten mindenhez (irás+olvasás)

#### Kezelő
* csak természetes adatokhoz férhet hozzá (irás+olvasás)

#### Kiemelt kezelő+Kezelő
* két előző együtt

#### Vezető
* cégszintű adatokhoz van olvasási hozzáférése

#### Semmi
* cégszintű adatokhoz nem férhet
Ha a rendszergazda cégszintű szerepe nem Semmi, a  cégszintű hozzá férések összadódnak a rendszergazdai hozzáféréssel. 

## Kódrendszerek kialakitása

Az egész rendszer alapvető célja a cég eredmények elemezhető kimutatása.
Eredmény: termék bevétele – termék költsége

A termékek és költségek megfelelő finomságú meghatározásával és ezek megfelelő hierarchiába rendezésén múlik a rendszer használhatósága.

Elemi termékként kell definiálni mindent, amire eredményt kivánunk látni. Természetesen lehet olyan elemi termékünk is, melyhez nem tartozik bevétel, (pl. általános rezsi, melyet nem tudok, vagy nem akarok szétosztani a többi termék között).  
Az elemi termék költségei az elemi költségek. Egy elemi termék csak egy elemi költséghez  rendelendő.

Mind a termékek, mind a költségek négyszintű hiererchába szervezhetőek:

* főcsoport
* alcsoport
* csoport
* elemi

Ez lehet, hogy sok.

A termék-/költségcsoportnak kitüntetett szerepe van az olyan számlák rögzitéséhez, ahol a számlaösszeget valamilyen, de állandó százalékos arányban szeretnénk tételsorokra bontani(séma). Ilyenek elsősorban a közüzemi számlák.

Ha egy partner lehet vevő, felvezetésekor kötelezően megadandó az elemi termékkód alapértelmezése, ha lehet szállitó, akkor az elemi költségkód alap-értelmezése. Mindkét esetben definiálható felosztási séma.

Új számla felvezetésekor a számla fejének OK-zásakor a program a megadottak alapján egy vagy séma esetén több tételsort rögzit. Ha (ami reményeim szerint ritkán fordul elő) változtatni akarunk a tételsoro(kon), erre van lehetőségünk, a fejrészen már nem, ha elrontottuk, törölni kell.

A Mi hiányzik? eligazit arról, hogy még mit kell felvenni. 
