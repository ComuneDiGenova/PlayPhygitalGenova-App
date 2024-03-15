# App Mobile Phygital

build version 1.0.4

L'applicazione è stata sviluppata utilizzando Unity e successivamente esportata per essere disponibile su dispositivi Android e iOS. Gli utenti hanno la possibilità di scegliere tra due modalità di accesso: accedere agli itinerari del comune senza registrazione o effettuare l'accesso tramite il servizio Sirac. Quest'ultimo rappresenta un servizio di autenticazione che permette l'accesso all'applicazione attraverso diversi provider come Spid, Google, Linkedin, e altri. Una volta scelto il provider di accesso preferito, l'utente procede con l'autenticazione e può quindi accedere al proprio profilo.

Nella schermata di accesso, l'utente ha anche la possibilità di selezionare la lingua di preferenza tramite un dropdown menu. Tale scelta della lingua può essere modificata in qualsiasi momento durante l'uso dell'app, accedendo alla sezione "Tuoi dati".

Dopo l'autenticazione, l'applicazione legge i dati dell'utente che vengono salvati nelle PlayerPrefs e avvia chiamate alle API di Drupal per recuperare informazioni quali i punti di interesse turistici (POI), i negozi e altre informazioni utili. Vengono effettuate anche chiamate al geoportale per ottenere ulteriori dettagli, come la posizione dei bagni pubblici e delle fermate del bus.

Una volta ottenuti questi dati, vengono istanziati i marker sulla mappa utilizzando il pacchetto Infinity Code. Questo consente agli utenti di visualizzare e esplorare i punti di interesse turistici, i negozi e altre informazioni utili direttamente all'interno dell'app. È importante sottolineare che i contenuti vengono recepiti mediante la serializzazione delle classi dai JSON ricevuti tramite le chiamate alle API di Drupal. La mappa interattiva mostra chiaramente la posizione geolocalizzata degli utenti e dei vari punti di interesse, offrendo un'esperienza di navigazione intuitiva e informativa.<br>

Successivamente vengono effettuate sempre delle chiamate alle API di Drupal per ottenere informazioni quali i preferiti dell'utente, gli itinerari del comune e quelli personalizzati e i punti genovini.<br>

L'applicazione inoltre ha una funzione di ricerca "vicino a me" che permette di poter visualizzare tutti i poi e i negozi in prossimità dell'utente.<br>

E stata implementata inoltre una sezione dedicata alla ricerca del POI, per permettere all'utente di poter eseguire una ricerca sui punti di interesse disponibili nella app.<br>

Ogni volta che l'utente avrà una interazione con un POI questo aprirà prima un'anteprima del contenuto e se l'utente clicca sul button specifico, può eccedere all'approfondimento del medesimo e se disponibile poter ascoltare l'avatar virtuale che rappresenta da guida interattiva.

## Export per Android e iOS

> L'applicazione è stata progettata per garantire accessibilità su dispositivi Android e iOS. L'esportazione è un passo cruciale per distribuire l'app su entrambe le piattaforme in modo efficace e ottimizzato.

### Export per Android

> L'export per Android coinvolge diversi passaggi chiave per assicurare un'esperienza fluida per gli utenti su questa piattaforma.

* **Compilazione in Unity**: Inizialmente, il progetto Unity viene compilato per generare il codice sorgente in C# e i file asset necessari per Android.
* **Generazione dell'APK**: Dopo la compilazione, si genera un file APK (Android Package) contenente l'applicazione, i file asset e il codice C#. Questo file rappresenta l'app pronta per essere installata e eseguita su dispositivi Android.
* **Testing e Ottimizzazione**: Prima della distribuzione, l'APK viene testato su diversi dispositivi Android per garantire che funzioni correttamente e sia ottimizzato per varie configurazioni hardware e versioni di sistema operativo.

### Export per iOS

> L'esportazione per iOS segue un processo altrettanto cruciale, adattato alle specifiche di questa piattaforma.

* **Compilazione in Unity**: Analogamente a Android, il progetto Unity viene compilato per generare il codice sorgente in C# e i file asset per iOS.
* **Generazione del file Xcode**: Dopo la compilazione, si genera un progetto Xcode che contiene il codice sorgente, le risorse e le configurazioni necessarie per l'app iOS.
* **Compilazione in Xcode**: Nel ambiente di sviluppo Xcode, si compila l'applicazione per generare un file IPA (iOS App Store Package), che è l'equivalente dell'APK per dispositivi iOS.
* **Testing su Dispositivi iOS**: L'IPA viene testata su dispositivi iOS per garantire che funzioni senza problemi e sia ottimizzata per le diverse versioni di iPhone e iPad.

> Entrambi questi processi di export sono fondamentali per fornire un'esperienza ottimizzata e coerente dell'app su Android e iOS.

## Struttura

> Questa app permette la fruizione dei contentuti di playphygital durante la visita a genova e guadagnare punti genovini.<br>

La lettura di questo documento è atta a comprendre tutti i passaggi e gli elementi che compongono la applicazione.<br>

Tutti gli elementi cruciali sono integrati in un'unica scena Unity.<br>

I passaggi chiave del progetto sono:

* Spript classi serializzazione
* Impostazione della lingua tramite Dropdown
* Richiesta dati utente tramite Sirac
* Richiesta InfoUser e salvataggio PlayerPrefs
* WSO2
* Commerciante
* Richiesta preferiti utente
* Richiesta Informazioni Utili Geoportale
* Caricamento della categoria dei POI con relative icone
* Caricamento dei POI turistici
* Caricamento dei negozi
* Vicino a Me
* Genovini
* Avatar
* Geolocalizzazione
* Percorsi
* Gestione Scaling Marker Mappa

## Features

### Controllo di versione della App

> Abbiamo introdotto un controllo di versione della app per fare in modo che questa ogni volta che viene avviata controlli se la versione attuale è quella piu aggiornata.

Lo script che gestisce questo controllo è:

```
public class VersionManager : MonoBehaviour
```

La applicazione fa una chiamata API ad endpoint specifico che restituisce un Json con il numero di versione, se il numero di versione restituito è superiore a quello di versione della applicazione, comparirà un panel con un button con il link dell'appStore su android e del playStore su IOS per poter aggiornare la versione.

N.B. Se non si aggiorna alla versione piu recente non è possibile poter utilizzare l'app.

### AR

> Augmented Reality (AR) è una tecnologia che sovrappone elementi digitali (come immagini, video, suoni) al mondo reale, migliorando la percezione e l'interazione degli utenti con l'ambiente circostante attraverso dispositivi come smartphone, tablet o occhiali appositi.

Extended Reality (XR) è un termine ombrello che comprende tutte le tecnologie immersive, come AR (Augmented Reality) e VR (Virtual Reality). XR crea un'esperienza più coinvolgente combinando il mondo reale con elementi virtuali.

Per quanto riguarda le specifiche minime su Android e iOS per la funzionalità AR:

**Android:**

* **Sistema Operativo:** Android 7.0 (Nougat) o versioni successive.
* **Processore:** CPU con supporto per ARCore (Google's ARCore) che include l'API di ARCore.
* **Memoria:** La quantità di RAM richiesta può variare in base all'applicazione AR specifica, ma almeno 3-4 GB di RAM sono raccomandati.
* **Sensori:** Giroscopio, accelerometro, fotocamera con supporto per ARCore.

**iOS:**

* **Sistema Operativo:** iOS 11 o versioni successive.
* **Processore:** A9 o successivo (A9, A10, A11, A12, ecc.).
* **Memoria:** Almeno 2 GB di RAM.
* **Sensori:** Giroscopio, accelerometro, fotocamera posteriore (preferibilmente con capacità di rilevamento di profondità, come TrueDepth Camera su iPhone X e successivi).

## Packages

Per lo sviluppo dell'applicativo abbiamo fatto uso del pacchetto offerto da Infinity Code: Online Maps<br>

Online Maps è una soluzione multipiattaforma universale per la creazione di mappe 2D e 3D in Unity.<br>

Completamente personalizzabile, incredibilmente facile da apprendere e utilizzare e allo stesso tempo è una delle soluzioni più potenti e flessibili del settore.<br>

Supporta un numero enorme di servizi per qualsiasi esigenza e si integra con le migliori risorse di Asset Store.

* [[https://infinity-code.com/assets/online-maps](https://infinity-code.com/assets/online-maps)] - Infinity Code Online Maps!

### Spript classi serializzazione

Lo script sviluppato contiene una serie di classi dedicate alla serializzazione dei dati JSON provenienti dalle chiamate API di Drupal. Queste classi permettono di strutturare e convertire in modo efficiente i dati JSON in oggetti utilizzabili nell'applicazione. Ogni classe è progettata per rappresentare un'entità specifica del sistema Drupal, come utenti, articoli o categorie. L'obiettivo principale di queste classi è garantire una gestione agevole e accurata delle informazioni ottenute dalle API, facilitando così l'integrazione dei dati provenienti da Drupal all'interno dell'applicazione.

```
WebServiceDefinition.cs
```

### Impostazione della lingua tramite Dropdown

Questa parte è accessibile nello script:

```
LanguageDropdown
```

Ogni contentuto intenro alla app è tradotto tramite lo script :

```
LocalizationText
```

che prende le informazioni sulla relativa traduzione tramite id contentuo dentro file Localisation.CSV salvato nella cartella Resources del progetto.

### Richiesta dati utente tramite Sirac

Ci connettiamo attraverso il browser del dispositivo utilizzando la pagina di autenticazione fornita da Sirac. Dopo aver completato la procedura di autenticazione, l'applicazione viene riaperta e riceve da Sirac un token di autenticazione. Tale token viene sottoposto a verifica per assicurare la sua validità e per ottenere l'email dell'utente e il codice fiscale associato.<br>

Successivamente, le API di Drupal rispondono indicando se esiste un utente registrato con le informazioni fornite o, in alternativa, restituiscono un codice di errore.

Questa parte è accessibile nello script:

```
public class LogInSSO : MonoBehaviour
```

Tutti i valori degli endpoint relativi all'autentitcazione di sirac si trovano nella calsse

```
public class SSO_Manager : MonoBehaviour
```

### Richiesta InfoUser e salvataggio PlayerPrefs

Utilizzando l'indirizzo email ottenuto, effettuiamo una richiesta di tipo GET all'endpoint "user-get-info". Questa chiamata ci restituirà le informazioni associate all'utente corrispondente all'indirizzo email fornito.<br>

Lo script è contentuo nel gameObject UserDataHandler nella classe

```
public class GETUserInfo : MonoBehaviour
```

### WSO2

Tutte le chiamate rivolte agli endpoint di Drupal avvengono tramite il servizio di proxy WSO2. Le credenziali, gli endpoint e la struttura delle chiamate sono definiti nella classe designata, che gestisce questa comunicazione con il servizio.<br>

Questa parte è accessibile nello script:

```
public class WSO2 : MonoBehaviour
```

### Commerciante

L'accesso del commerciante consente di accedere a una pagina dedicata in cui è possibile registrare gli acquisti dei clienti e assegnare loro i relativi punti fedeltà. Attraverso un semplice modulo, il commerciante inserisce il numero dello scontrino e altre informazioni. L'identificativo dell'utente può essere inserito manualmente o acquisito tramite scansione del QR code generato dall'applicazione del cliente in base al suo ID.

Lo script di riferimento si occupa di raccogliere i dati inseriti nel modulo e richiama la funzione GetUserInfo per ottenere le informazioni sull'utente e successivamente chiama il metodo AddPoints per aggiungere i punti al cliente.

```
public class SendRecepit : MonoBehaviour
public class GetUserInfo : MonoBehaviour
```

### Caricamento della categoria dei POI con relative icone

Questa parte è accessibile nello script:

```
public class GetTipologiePOI : MonoBehaviour
```

L'obiettivo principale di questo script è scaricare e visualizzare le tipologie dei POI nell'interfaccia utente, consentendo loro di essere selezionate e utilizzate nel contesto dell'applicazione, questo script crea un dictionary con le tipologie di POI e le relative PNG corrispondenti.

### Caricamento dei POI turistici

Questa parte è accessibile nello script:

```
public class GETPointOfInterest : MonoBehaviour
```

L'obiettivo principale di questo script è scaricare tutte le informazioni relative ai POI dal portale Drupal ed istanziare tutti i marker sulla mappa Tileset di Infinity Code rispettando i dati di geolocalizzazione quali latitudine e longitudine.

Gli endpoit di riferimento per le API sono i seguenti

```
static string poiDetailsEndpoint = <span class="hljs-string">"/post_poi_details"</span>;
static string poiListEndpoint = <span class="hljs-string">"/get_poi_list"</span>;
```

### Caricamento dei negozi

Questa parte è accessibile nello script:

```
public class GETShops : MonoBehaviour
```

L'obiettivo principale di questo script è scaricare tutte le informazioni relative ai POI dal portale Drupal ed istanziare tutti i marker sulla mappa Tileset di Infinity Code rispettando i dati di geolocalizzazione quali latitudine e longitudine.

Gli endpoit di riferimento per le API sono i seguenti

```
string apiURL = <span class="hljs-string">"/get_shop_list"</span>;
static string shopDeteilsEndpoint = <span class="hljs-string">"/post_shop_details"</span>;
```

### Vicino a me

> La funzione vicino a me viene gestita richiamado una funzione non statica al gameObject gameManager alla classe:

```
public class UIManager : MonoBehaviour
```

Richiamando il metodo: CallActiveNearMe

### Genovini

> La funzione genovini viene gestita richiamado una funzione non statica al gameObject gameManager alla classe:

```
public class UIManager : MonoBehaviour
```

Richiamando il metodo: CallActivePoints<br>

I campi relativi al punteggio dei genovini presenti in questa tab vengono richiamati dallo script

```
public class GETUserInfo : MonoBehaviour
```

> Il caricamento delle immagini avviene tramite Amazon Web Services (AWS) utilizzando l'oggetto JsonReader. Questo oggetto gestisce la richiesta e il download delle immagini in alta risoluzione dal servizio AWS. Quando il tour inizia o si sposta a una nuova posizione, il metodo EvaluateBuffer viene invocato per richiedere ad AWS le immagini in alta risoluzione associate alla posizione corrente o successiva. Successivamente, l'immagine scaricata viene applicata ai materiali delle sfere (sferaA e sferaB) tramite i rispettivi materiali e texture. Il processo di caricamento delle immagini in alta risoluzione avviene in modo asincrono, consentendo al tour di continuare una volta completato il caricamento delle immagini.

### Avatar

> All'interno dell'applicazione, sono implementati due avatar virtuali rappresentanti Paganini e la Duchessa di Galliera di Genova. Questi avatar svolgono il ruolo di guide virtuali durante l'esperienza di fruizione dei contenuti in Realtà Aumentata (AR), nonché durante la visualizzazione dei contenuti di approfondimento. Lo script che gestisce la selezione dell'avatar virtuale in base al contenuto è il seguente:

```
public class AvatarSelector : MonoBehaviour
```

La scelta dell'avatar avviene in base all'informazione che arriva dalla chimata alla APi di Drupal : post_poi_details

### Geolocalizzazione

Lo script "Location Service" di Infinity Code Online Maps è un componente essenziale per la geolocalizzazione all'interno di applicazioni che utilizzano questa piattaforma. Esso offre funzionalità avanzate per rilevare e gestire la posizione geografica dell'utente in tempo reale. Il servizio consente di accedere alle informazioni di posizione come latitudine, longitudine, altitudine e precisione, fornendo una base solida per l'integrazione di mappe interattive e applicazioni basate sulla localizzazione. Grazie a questa risorsa, è possibile creare esperienze coinvolgenti e personalizzate basate sulla posizione dell'utente, arricchendo l'interazione tra l'utente e l'applicazione tramite l'uso efficace delle informazioni geospaziali.

E' possibile trovare lo script descritto nel gameObject "Map_TILE" che contiene anche gli script Online Maps e online maps Marker Manager

### Percorsi

Lo script "RoutePercorso" è un componente in Unity che gestisce la visualizzazione e l'interpolazione di un percorso su una mappa. Esso consente di disegnare un percorso tra punti di interesse (POI) specificati e di ottenere coordinate intermedie per garantire una visualizzazione più fluida. Utilizza la piattaforma Infinity Code Online Maps per disegnare il percorso sulla mappa in base alle coordinate geografiche dei POI. Il componente offre anche la funzionalità di rimozione del percorso precedentemente disegnato.

**_maxWaypointDistance_**

* Descrizione: Specifica la massima distanza tra i punti del percorso.
* Modificabile dall'Inspector: Sì. Il valore può essere modificato direttamente dall'Inspector.<br>
percorsoDemo

**_percorsoDemo_**

* Descrizione: Abilita o disabilita l'uso di un percorso demo.
* Modificabile dall'Inspector: Sì. Può essere attivato o disattivato dall'Inspector.

**_lineColor_**

* Descrizione: Specifica il colore della linea del percorso sulla mappa.
* Modificabile dall'Inspector: Sì. Il colore può essere selezionato tramite il selettore di colore nell'Inspector.

**_lineWidth_**

* Descrizione: Specifica la larghezza della linea del percorso sulla mappa.
* Modificabile dall'Inspector: Sì. Il valore può essere modificato direttamente dall'Inspector.

### Gestione Scaling Marker Mappa

```
public class MarkerScaleByZoom  : MonoBehaviour
```

Lo script progettato per cambiare la dimensione di alcuni marcatori sulla mappa in base allo zoom. Viene specificato un valore di zoom predefinito (18) e un valore minimo (10). Se lo zoom è inferiore al valore minimo, la dimensione dei marcatori viene ridotta al 50%. Se lo zoom supera il valore predefinito, la dimensione dei marcatori rimane normale. Durante il cambio dello zoom, il codice calcola la scala adeguata per i marcatori in base all'intervallo specificato e la applica.

* defaultZoom: Rappresenta il valore predefinito dello zoom sulla mappa. Può variare tra 1 e 22.
* minZoom: Indica il valore minimo dello zoom consentito sulla mappa. Può variare tra 1 e 22, ma è limitato a non superare il valore di defaultZoom.
* minScale: Rappresenta la scala minima che sarà applicata ai marcatori quando lo zoom è inferiore al valore di minZoom. La scala può variare tra 0.1 (10%) e 1 (100%).
* markerScales: È un dizionario che tiene traccia delle scale dei marcatori prima di eventuali modifiche, associando ciascun marcatore alla sua scala.
