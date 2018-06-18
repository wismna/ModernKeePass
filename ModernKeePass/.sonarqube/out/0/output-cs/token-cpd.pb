§
GC:\Sources\Other\ModernKeePass\ModernKeePass\Actions\ClipboardAction.cs
	namespace 	
ModernKeePass
 
. 
Actions 
{ 
public 

class 
ClipboardAction  
:! "
DependencyObject# 3
,3 4
IAction5 <
{ 
public		 
string		 
Text		 
{

 	
get 
{ 
return 
( 
string  
)  !
GetValue! )
() *
TextProperty* 6
)6 7
;7 8
}9 :
set 
{ 
SetValue 
( 
TextProperty '
,' (
value) .
). /
;/ 0
}1 2
} 	
public 
static 
readonly 
DependencyProperty 1
TextProperty2 >
=? @
DependencyProperty 
. 
Register '
(' (
$str( .
,. /
typeof0 6
(6 7
string7 =
)= >
,> ?
typeof@ F
(F G
ClipboardActionG V
)V W
,W X
newY \
PropertyMetadata] m
(m n
stringn t
.t u
Emptyu z
)z {
){ |
;| }
public 
object 
Execute 
( 
object $
sender% +
,+ ,
object- 3
	parameter4 =
)= >
{ 	
var 
dataPackage 
= 
new !
DataPackage" -
{. /
RequestedOperation0 B
=C D 
DataPackageOperationE Y
.Y Z
CopyZ ^
}_ `
;` a
dataPackage 
. 
SetText 
(  
Text  $
)$ %
;% &
	Clipboard 
. 

SetContent  
(  !
dataPackage! ,
), -
;- .
return 
null 
; 
} 	
} 
} œ
KC:\Sources\Other\ModernKeePass\ModernKeePass\Actions\NavigateToUrlAction.cs
	namespace 	
ModernKeePass
 
. 
Actions 
{ 
public 

class 
NavigateToUrlAction $
:% &
DependencyObject' 7
,7 8
IAction9 @
{		 
public

 
string

 
Url

 
{ 	
get 
{ 
return 
( 
string  
)  !
GetValue! )
() *
UrlProperty* 5
)5 6
;6 7
}8 9
set 
{ 
SetValue 
( 
UrlProperty &
,& '
value( -
)- .
;. /
}0 1
} 	
public 
static 
readonly 
DependencyProperty 1
UrlProperty2 =
=> ?
DependencyProperty 
. 
Register '
(' (
$str( -
,- .
typeof/ 5
(5 6
string6 <
)< =
,= >
typeof? E
(E F
NavigateToUrlActionF Y
)Y Z
,Z [
new\ _
PropertyMetadata` p
(p q
stringq w
.w x
Emptyx }
)} ~
)~ 
;	 Ä
public 
object 
Execute 
( 
object $
sender% +
,+ ,
object- 3
	parameter4 =
)= >
{ 	
try 
{ 
var 
uri 
= 
new 
Uri !
(! "
Url" %
)% &
;& '
return 
Windows 
. 
System %
.% &
Launcher& .
.. /
LaunchUriAsync/ =
(= >
uri> A
)A B
.B C

GetAwaiterC M
(M N
)N O
.O P
	GetResultP Y
(Y Z
)Z [
;[ \
} 
catch 
( 
	Exception 
ex 
)  
{ 
MessageDialogHelper #
.# $
ShowErrorDialog$ 3
(3 4
ex4 6
)6 7
;7 8
return 
false 
; 
} 
} 	
}   
}!! ø
HC:\Sources\Other\ModernKeePass\ModernKeePass\Actions\SetupFocusAction.cs
	namespace 	
ModernKeePass
 
. 
Actions 
{ 
public		 

class		 
SetupFocusAction		 !
:		" #
DependencyObject		$ 4
,		4 5
IAction		6 =
{

 
public 
Control 
TargetObject #
{ 	
get 
{ 
return 
( 
Control !
)! "
GetValue" *
(* + 
TargetObjectProperty+ ?
)? @
;@ A
}B C
set 
{ 
SetValue 
(  
TargetObjectProperty /
,/ 0
value1 6
)6 7
;7 8
}9 :
} 	
public 
static 
readonly 
DependencyProperty 1 
TargetObjectProperty2 F
=G H
DependencyProperty 
. 
Register '
(' (
$str( 6
,6 7
typeof8 >
(> ?
Control? F
)F G
,G H
typeofI O
(O P
SetupFocusActionP `
)` a
,a b
newc f
PropertyMetadatag w
(w x
nullx |
)| }
)} ~
;~ 
public 
object 
Execute 
( 
object $
sender% +
,+ ,
object- 3
	parameter4 =
)= >
{ 	
return 
Task 
. 
Factory 
.  
StartNew  (
(( )
( 
) 
=> 

Dispatcher  
.  !
RunAsync! )
() *"
CoreDispatcherPriority* @
.@ A
LowA D
,D E
( 
) 
=> 
TargetObject &
?& '
.' (
Focus( -
(- .

FocusState. 8
.8 9
Programmatic9 E
)E F
)F G
)G H
;H I
} 	
} 
} Ω\
8C:\Sources\Other\ModernKeePass\ModernKeePass\App.xaml.cs
	namespace 	
ModernKeePass
 
{ 
sealed 

partial 
class 
App 
{ 
public 
App 
( 
) 
{ 	
HockeyClient 
. 
Current  
.  !
	Configure! *
(* +
$str+ M
)M N
;N O
InitializeComponent 
(  
)  !
;! "

Suspending   
+=   
OnSuspending   &
;  & '
Resuming!! 
+=!! 

OnResuming!! "
;!!" #
UnhandledException"" 
+="" ! 
OnUnhandledException""" 6
;""6 7
}## 	
private(( 
void((  
OnUnhandledException(( )
((() *
object((* 0
sender((1 7
,((7 8'
UnhandledExceptionEventArgs((9 T'
unhandledExceptionEventArgs((U p
)((p q
{)) 	
var++ 
	exception++ 
=++ '
unhandledExceptionEventArgs++ 7
.++7 8
	Exception++8 A
;++A B
var,, 
realException,, 
=,, 
	exception-- 
is-- %
TargetInvocationException-- 6
&&--7 9
	exception.. 
... 
InnerException.. (
!=..) +
null.., 0
?// 
	exception// 
.//  
InnerException//  .
:00 
	exception00 
;00  
if22 
(22 
realException22 
is22  
SaveException22! .
)22. /
{33 '
unhandledExceptionEventArgs44 +
.44+ ,
Handled44, 3
=444 5
true446 :
;44: ;
MessageDialogHelper55 #
.55# $
SaveErrorDialog55$ 3
(553 4
realException554 A
as55B D
SaveException55E R
,55R S
DatabaseService55T c
.55c d
Instance55d l
)55l m
;55m n
}66 
else77 
if77 
(77 
realException77 "
is77# %#
DatabaseOpenedException77& =
)77= >
{88 '
unhandledExceptionEventArgs99 +
.99+ ,
Handled99, 3
=994 5
true996 :
;99: ;
MessageDialogHelper:: #
.::# $
SaveUnchangedDialog::$ 7
(::7 8
realException::8 E
as::F H#
DatabaseOpenedException::I `
,::` a
DatabaseService::b q
.::q r
Instance::r z
)::z {
;::{ |
};; 
}<< 	
	protectedCC 
overrideCC 
asyncCC  
voidCC! %

OnLaunchedCC& 0
(CC0 1$
LaunchActivatedEventArgsCC1 I
eCCJ K
)CCK L
{DD 	
OnLaunchOrActivatedEE 
(EE  
eEE  !
)EE! "
;EE" #
awaitFF 
HockeyClientFF 
.FF 
CurrentFF &
.FF& '
SendCrashesAsyncFF' 7
(FF7 8
)FFU V
;FFV W
}GG 	
	protectedII 
overrideII 
voidII 
OnActivatedII  +
(II+ ,
IActivatedEventArgsII, ?
argsII@ D
)IID E
{JJ 	
OnLaunchOrActivatedKK 
(KK  
argsKK  $
)KK$ %
;KK% &
}LL 	
privateNN 
voidNN 
OnLaunchOrActivatedNN (
(NN( )
IActivatedEventArgsNN) <
eNN= >
)NN> ?
{OO 	
ifRR 
(RR 
SystemRR 
.RR 
DiagnosticsRR "
.RR" #
DebuggerRR# +
.RR+ ,

IsAttachedRR, 6
)RR6 7
{SS 
}UU 
varXX 
	rootFrameXX 
=XX 
WindowXX "
.XX" #
CurrentXX# *
.XX* +
ContentXX+ 2
asXX3 5
FrameXX6 ;
;XX; <
if\\ 
(\\ 
	rootFrame\\ 
==\\ 
null\\ !
)\\! "
{]] 
	rootFrame__ 
=__ 
new__ 
Frame__  %
{__& '
Language__' /
=__0 1
Windows__2 9
.__9 :
Globalization__: G
.__G H 
ApplicationLanguages__H \
.__\ ]
	Languages__] f
[__f g
$num__g h
]__h i
}__i j
;__j k
	rootFramebb 
.bb 
NavigationFailedbb *
+=bb+ -
OnNavigationFailedbb. @
;bb@ A
ifdd 
(dd 
edd 
.dd "
PreviousExecutionStatedd ,
==dd- /%
ApplicationExecutionStatedd0 I
.ddI J

TerminatedddJ T
)ddT U
{ee 
MessageDialogHelperhh '
.hh' ("
ShowNotificationDialoghh( >
(hh> ?
$strhh? O
,hhO P
$strhhQ }
)hh} ~
;hh~ 
}jj 
Windowmm 
.mm 
Currentmm 
.mm 
Contentmm &
=mm' (
	rootFramemm) 2
;mm2 3
}nn 
ifpp 
(pp 
epp 
ispp $
LaunchActivatedEventArgspp -
)pp- .
{qq 
varrr #
lauchActivatedEventArgsrr +
=rr, -
(rr. /$
LaunchActivatedEventArgsrr/ G
)rrG H
errI J
;rrJ K
ifss 
(ss 
	rootFramess 
.ss 
Contentss %
==ss& (
nullss) -
)ss- .
{tt 
	rootFramexx 
.xx 
Navigatexx &
(xx& '
typeofxx' -
(xx- .
MainPagexx. 6
)xx6 7
,xx7 8#
lauchActivatedEventArgsxx9 P
.xxP Q
	ArgumentsxxQ Z
)xxZ [
;xx[ \
}yy 
} 
Window
ââ 
.
ââ 
Current
ââ 
.
ââ 
Activate
ââ #
(
ââ# $
)
ââ$ %
;
ââ% &
}
ää 	
private
åå 
async
åå 
void
åå 

OnResuming
åå %
(
åå% &
object
åå& ,
sender
åå- 3
,
åå3 4
object
åå5 ;
e
åå< =
)
åå= >
{
çç 	
var
éé 
currentFrame
éé 
=
éé 
Window
éé %
.
éé% &
Current
éé& -
.
éé- .
Content
éé. 5
as
éé6 8
Frame
éé9 >
;
éé> ?
var
èè 
database
èè 
=
èè 
DatabaseService
èè *
.
èè* +
Instance
èè+ 3
;
èè3 4
if
êê 
(
êê 
database
êê 
.
êê 
DatabaseFile
êê %
==
êê& (
null
êê) -
)
êê- .
{
ëë %
ToastNotificationHelper
ìì '
.
ìì' (
ShowGenericToast
ìì( 8
(
ìì8 9
$str
ìì9 H
,
ììH I
$str
ììJ v
)
ììv w
;
ììw x
return
ïï 
;
ïï 
}
ññ 
try
óó 
{
òò 
if
ôô 
(
ôô 
database
ôô 
.
ôô 
CompositeKey
ôô )
!=
ôô* ,
null
ôô- 1
)
ôô1 2
await
ôô3 8
database
ôô9 A
.
ôôA B
ReOpen
ôôB H
(
ôôH I
)
ôôI J
;
ôôJ K
}
öö 
catch
õõ 
(
õõ 
	Exception
õõ 
ex
õõ 
)
õõ  
{
úú 
currentFrame
ùù 
?
ùù 
.
ùù 
Navigate
ùù &
(
ùù& '
typeof
ùù' -
(
ùù- .
MainPage
ùù. 6
)
ùù6 7
)
ùù7 8
;
ùù8 9!
MessageDialogHelper
üü #
.
üü# $
ShowErrorDialog
üü$ 3
(
üü3 4
ex
üü4 6
)
üü6 7
;
üü7 8%
ToastNotificationHelper
°° '
.
°°' (
ShowGenericToast
°°( 8
(
°°8 9
$str
°°9 H
,
°°H I
$str
°°J t
)
°°t u
;
°°u v
}
¢¢ 
}
££ 	
void
™™  
OnNavigationFailed
™™ 
(
™™  
object
™™  &
sender
™™' -
,
™™- .'
NavigationFailedEventArgs
™™/ H
e
™™I J
)
™™J K
{
´´ 	
throw
¨¨ 
new
¨¨ 
	Exception
¨¨ 
(
¨¨  
$str
¨¨  6
+
¨¨7 8
e
¨¨9 :
.
¨¨: ;
SourcePageType
¨¨; I
.
¨¨I J
FullName
¨¨J R
)
¨¨R S
;
¨¨S T
}
≠≠ 	
private
∂∂ 
async
∂∂ 
void
∂∂ 
OnSuspending
∂∂ '
(
∂∂' (
object
∂∂( .
sender
∂∂/ 5
,
∂∂5 6!
SuspendingEventArgs
∂∂7 J
e
∂∂K L
)
∂∂L M
{
∑∑ 	
var
∏∏ 
deferral
∏∏ 
=
∏∏ 
e
∏∏ 
.
∏∏ !
SuspendingOperation
∏∏ 0
.
∏∏0 1
GetDeferral
∏∏1 <
(
∏∏< =
)
∏∏= >
;
∏∏> ?
var
ππ 
database
ππ 
=
ππ 
DatabaseService
ππ *
.
ππ* +
Instance
ππ+ 3
;
ππ3 4
try
∫∫ 
{
ªª 
if
ºº 
(
ºº 
SettingsService
ºº #
.
ºº# $
Instance
ºº$ ,
.
ºº, -

GetSetting
ºº- 7
(
ºº7 8
$str
ºº8 E
,
ººE F
true
ººG K
)
ººK L
)
ººL M
database
ººN V
.
ººV W
Save
ººW [
(
ºº[ \
)
ºº\ ]
;
ºº] ^
await
ΩΩ 
database
ΩΩ 
.
ΩΩ 
Close
ΩΩ $
(
ΩΩ$ %
false
ΩΩ% *
)
ΩΩ* +
;
ΩΩ+ ,
}
ææ 
catch
øø 
(
øø 
	Exception
øø 
	exception
øø &
)
øø& '
{
¿¿ %
ToastNotificationHelper
¬¬ '
.
¬¬' (
ShowErrorToast
¬¬( 6
(
¬¬6 7
	exception
¬¬7 @
)
¬¬@ A
;
¬¬A B
}
ƒƒ 
deferral
≈≈ 
.
≈≈ 
Complete
≈≈ 
(
≈≈ 
)
≈≈ 
;
≈≈  
}
∆∆ 	
	protected
ÃÃ 
override
ÃÃ 
void
ÃÃ 
OnFileActivated
ÃÃ  /
(
ÃÃ/ 0$
FileActivatedEventArgs
ÃÃ0 F
args
ÃÃG K
)
ÃÃK L
{
ÕÕ 	
base
ŒŒ 
.
ŒŒ 
OnFileActivated
ŒŒ  
(
ŒŒ  !
args
ŒŒ! %
)
ŒŒ% &
;
ŒŒ& '
var
œœ 
	rootFrame
œœ 
=
œœ 
new
œœ 
Frame
œœ  %
(
œœ% &
)
œœ& '
;
œœ' (
DatabaseService
–– 
.
–– 
Instance
–– $
.
––$ %
DatabaseFile
––% 1
=
––2 3
args
––4 8
.
––8 9
Files
––9 >
[
––> ?
$num
––? @
]
––@ A
as
––B D
StorageFile
––E P
;
––P Q
	rootFrame
—— 
.
—— 
Navigate
—— 
(
—— 
typeof
—— %
(
——% &
MainPage
——& .
)
——. /
,
——/ 0
args
——1 5
)
——5 6
;
——6 7
Window
““ 
.
““ 
Current
““ 
.
““ 
Content
““ "
=
““# $
	rootFrame
““% .
;
““. /
Window
”” 
.
”” 
Current
”” 
.
”” 
Activate
”” #
(
””# $
)
””$ %
;
””% &
}
‘‘ 	
}
◊◊ 
}ÿÿ ŒX
OC:\Sources\Other\ModernKeePass\ModernKeePass\Converters\IntToSymbolConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class  
IntToSymbolConverter %
:& '
IValueConverter( 7
{		 
public

 
object

 
Convert

 
(

 
object

 $
value

% *
,

* +
Type

, 0

targetType

1 ;
,

; <
object

= C
	parameter

D M
,

M N
string

O U
language

V ^
)

^ _
{ 	
var 
icon 
= 
( 
PwIcon 
) 
value  %
;% &
switch 
( 
icon 
) 
{ 
case 
PwIcon 
. 
Key 
:  
return! '
Symbol( .
.. /
Permissions/ :
;: ;
case 
PwIcon 
. 
WorldSocket '
:' (
case 
PwIcon 
. 
World !
:! "
return# )
Symbol* 0
.0 1
World1 6
;6 7
case 
PwIcon 
. 
Warning #
:# $
return% +
Symbol, 2
.2 3
	Important3 <
;< =
case 
PwIcon 
. 
WorldComputer )
:) *
case 
PwIcon 
. 
Drive !
:! "
case 
PwIcon 
. 
DriveWindows (
:( )
case 
PwIcon 
. 
NetworkServer )
:) *
return+ 1
Symbol2 8
.8 9
MapDrive9 A
;A B
case 
PwIcon 
. 
MarkedDirectory +
:+ ,
return- 3
Symbol4 :
.: ;
Map; >
;> ?
case 
PwIcon 
. 
UserCommunication -
:- .
return/ 5
Symbol6 <
.< =
ContactInfo= H
;H I
case 
PwIcon 
. 
Parts !
:! "
return# )
Symbol* 0
.0 1
ViewAll1 8
;8 9
case 
PwIcon 
. 
Notepad #
:# $
return% +
Symbol, 2
.2 3
Document3 ;
;; <
case 
PwIcon 
. 
Identity $
:$ %
return& ,
Symbol- 3
.3 4
Contact24 <
;< =
case 
PwIcon 
. 

PaperReady &
:& '
return( .
Symbol/ 5
.5 6

SyncFolder6 @
;@ A
case 
PwIcon 
. 
Digicam #
:# $
return% +
Symbol, 2
.2 3
Camera3 9
;9 :
case 
PwIcon 
. 
IRCommunication +
:+ ,
return- 3
Symbol4 :
.: ;
View; ?
;? @
case 
PwIcon 
. 
Energy "
:" #
return$ *
Symbol+ 1
.1 2
ZeroBars2 :
;: ;
case   
PwIcon   
.   
Scanner   #
:  # $
return  % +
Symbol  , 2
.  2 3
Scan  3 7
;  7 8
case!! 
PwIcon!! 
.!! 
CDRom!! !
:!!! "
return!!# )
Symbol!!* 0
.!!0 1
Rotate!!1 7
;!!7 8
case"" 
PwIcon"" 
."" 
Monitor"" #
:""# $
return""% +
Symbol"", 2
.""2 3
Caption""3 :
;"": ;
case## 
PwIcon## 
.## 
EMailBox## $
:##$ %
case$$ 
PwIcon$$ 
.$$ 
EMail$$ !
:$$! "
return$$# )
Symbol$$* 0
.$$0 1
Mail$$1 5
;$$5 6
case%% 
PwIcon%% 
.%% 
Configuration%% )
:%%) *
return%%+ 1
Symbol%%2 8
.%%8 9
Setting%%9 @
;%%@ A
case&& 
PwIcon&& 
.&& 
ClipboardReady&& *
:&&* +
return&&, 2
Symbol&&3 9
.&&9 :
Paste&&: ?
;&&? @
case'' 
PwIcon'' 
.'' 
PaperNew'' $
:''$ %
return''& ,
Symbol''- 3
.''3 4
Page2''4 9
;''9 :
case(( 
PwIcon(( 
.(( 
Screen(( "
:((" #
return(($ *
Symbol((+ 1
.((1 2
	GoToStart((2 ;
;((; <
case)) 
PwIcon)) 
.)) 
EnergyCareful)) )
:))) *
return))+ 1
Symbol))2 8
.))8 9
FourBars))9 A
;))A B
case** 
PwIcon** 
.** 
Disk**  
:**  !
return**" (
Symbol**) /
.**/ 0
Save**0 4
;**4 5
case-- 
PwIcon-- 
.-- 
Console-- #
:--# $
return--% +
Symbol--, 2
.--2 3
	SlideShow--3 <
;--< =
case.. 
PwIcon.. 
... 
Printer.. #
:..# $
return..% +
Symbol.., 2
...2 3
Scan..3 7
;..7 8
case// 
PwIcon// 
.// 
ProgramIcons// (
://( )
return//* 0
Symbol//1 7
.//7 8
	GoToStart//8 A
;//A B
case11 
PwIcon11 
.11 
Settings11 $
:11$ %
case22 
PwIcon22 
.22 
Tool22  
:22  !
return22" (
Symbol22) /
.22/ 0
Repair220 6
;226 7
case33 
PwIcon33 
.33 
Archive33 #
:33# $
return33% +
Symbol33, 2
.332 3
Crop333 7
;337 8
case44 
PwIcon44 
.44 
Count44 !
:44! "
return44# )
Symbol44* 0
.440 1

Calculator441 ;
;44; <
case55 
PwIcon55 
.55 
Clock55 !
:55! "
return55# )
Symbol55* 0
.550 1
Clock551 6
;556 7
case66 
PwIcon66 
.66 
EMailSearch66 '
:66' (
return66) /
Symbol660 6
.666 7
Find667 ;
;66; <
case77 
PwIcon77 
.77 
	PaperFlag77 %
:77% &
return77' -
Symbol77. 4
.774 5
Flag775 9
;779 :
case99 
PwIcon99 
.99 
TrashBin99 $
:99$ %
return99& ,
Symbol99- 3
.993 4
Delete994 :
;99: ;
case:: 
PwIcon:: 
.:: 
Expired:: #
:::# $
return::% +
Symbol::, 2
.::2 3
Cancel::3 9
;::9 :
case;; 
PwIcon;; 
.;; 
Info;;  
:;;  !
return;;" (
Symbol;;) /
.;;/ 0
Help;;0 4
;;;4 5
case== 
PwIcon== 
.== 
Folder== "
:==" #
case>> 
PwIcon>> 
.>> 

FolderOpen>> &
:>>& '
case?? 
PwIcon?? 
.?? 
FolderPackage?? )
:??) *
return??+ 1
Symbol??2 8
.??8 9
Folder??9 ?
;??? @
caseAA 
PwIconAA 
.AA 
PaperLockedAA '
:AA' (
returnAA) /
SymbolAA0 6
.AA6 7
ProtectedDocumentAA7 H
;AAH I
caseBB 
PwIconBB 
.BB 
CheckedBB #
:BB# $
returnBB% +
SymbolBB, 2
.BB2 3
AcceptBB3 9
;BB9 :
caseCC 
PwIconCC 
.CC 
PenCC 
:CC  
returnCC! '
SymbolCC( .
.CC. /
EditCC/ 3
;CC3 4
caseDD 
PwIconDD 
.DD 
	ThumbnailDD %
:DD% &
returnDD' -
SymbolDD. 4
.DD4 5
BrowsePhotosDD5 A
;DDA B
caseEE 
PwIconEE 
.EE 
BookEE  
:EE  !
returnEE" (
SymbolEE) /
.EE/ 0
LibraryEE0 7
;EE7 8
caseFF 
PwIconFF 
.FF 
ListFF  
:FF  !
returnFF" (
SymbolFF) /
.FF/ 0
ListFF0 4
;FF4 5
caseGG 
PwIconGG 
.GG 
UserKeyGG #
:GG# $
returnGG% +
SymbolGG, 2
.GG2 3
ContactPresenceGG3 B
;GGB C
caseHH 
PwIconHH 
.HH 
HomeHH  
:HH  !
returnHH" (
SymbolHH) /
.HH/ 0
HomeHH0 4
;HH4 5
caseII 
PwIconII 
.II 
StarII  
:II  !
returnII" (
SymbolII) /
.II/ 0
OutlineStarII0 ;
;II; <
caseNN 
PwIconNN 
.NN 
MoneyNN !
:NN! "
returnNN# )
SymbolNN* 0
.NN0 1
ShopNN1 5
;NN5 6
caseOO 
PwIconOO 
.OO 
CertificateOO '
:OO' (
returnOO) /
SymbolOO0 6
.OO6 7
PreviewLinkOO7 B
;OOB C
casePP 
PwIconPP 
.PP 

BlackBerryPP &
:PP& '
returnPP( .
SymbolPP/ 5
.PP5 6
	CellPhonePP6 ?
;PP? @
defaultQQ 
:QQ 
returnQQ 
SymbolQQ  &
.QQ& '
StopQQ' +
;QQ+ ,
}RR 
}SS 	
publicUU 
objectUU 
ConvertBackUU !
(UU! "
objectUU" (
valueUU) .
,UU. /
TypeUU0 4

targetTypeUU5 ?
,UU? @
objectUUA G
	parameterUUH Q
,UUQ R
stringUUS Y
languageUUZ b
)UUb c
{VV 	
varWW 
symbolWW 
=WW 
(WW 
SymbolWW  
)WW  !
valueWW" '
;WW' (
switchXX 
(XX 
symbolXX 
)XX 
{YY 
casehh 
Symbolhh 
.hh 
Deletehh "
:hh" #
returnii 
PwIconii !
.ii! "
TrashBinii" *
;ii* +
default
‹‹ 
:
‹‹ 
return
›› 
PwIcon
›› !
.
››! "
Folder
››" (
;
››( )
}
ﬁﬁ 
}
ﬂﬂ 	
}
‡‡ 
}·· ’
RC:\Sources\Other\ModernKeePass\ModernKeePass\Exceptions\DatabaseOpenedException.cs
	namespace 	
ModernKeePass
 
. 

Exceptions "
{ 
public 

class #
DatabaseOpenedException (
:( )
	Exception* 3
{ 
} 
}		 †
JC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\ILicenseService.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface 
ILicenseService $
{ 
IReadOnlyDictionary		 
<		 
string		 "
,		" #
ProductListing		$ 2
>		2 3
Products		4 <
{		= >
get		? B
;		B C
}		D E
Task

 
<

 
int

 
>

 
Purchase

 
(

 
string

 !
addOn

" '
)

' (
;

( )
} 
} √
RC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\IProxyInvocationHandler.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface #
IProxyInvocationHandler ,
{ 
object 
Invoke 
( 
object 
proxy "
," #

MethodInfo$ .
method/ 5
,5 6
object7 =
[= >
]> ?

parameters@ J
)J K
;K L
} 
}		 Ω
IC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\IRecentService.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface 
IRecentService #
{ 
int		 

EntryCount		 
{		 
get		 
;		 
}		 
Task

 
<

 
IStorageItem

 
>

 
GetFileAsync

 '
(

' (
string

( .
token

/ 4
)

4 5
;

5 6 
ObservableCollection 
< 
IRecentItem (
>( )
GetAllFiles* 5
(5 6
bool6 :
removeIfNonExistant; N
=O P
trueQ U
)U V
;V W
void 
Add 
( 
IStorageItem 
file "
," #
string$ *
metadata+ 3
)3 4
;4 5
void 
ClearAll 
( 
) 
; 
} 
} ’
FC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\IRecentItem.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface 
IRecentItem  
{ 
StorageFile 
DatabaseFile  
{! "
get# &
;& '
}( )
string 
Token 
{ 
get 
; 
} 
string		 
Name		 
{		 
get		 
;		 
}		 
}

 
} ©
KC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\IResourceService.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface 
IResourceService %
{ 
string 
GetResourceValue 
(  
string  &
key' *
)* +
;+ ,
} 
} Ä
MC:\Sources\Other\ModernKeePass\ModernKeePass\Services\SingletonServiceBase.cs
	namespace 	
ModernKeePass
 
. 
Services  
{ 
public 

abstract 
class  
SingletonServiceBase .
<. /
T/ 0
>0 1
where2 7
T8 9
:: ;
new< ?
(? @
)@ A
{ 
private 
static 
readonly 
Lazy  $
<$ %
T% &
>& '
LazyInstance( 4
=5 6
new 
Lazy 
< 
T 
> 
( 
( 
) 
=> 
new !
T" #
(# $
)$ %
)% &
;& '
public

 
static

 
T

 
Instance

  
=>

! #
LazyInstance

$ 0
.

0 1
Value

1 6
;

6 7
} 
} ﬁ

`C:\Sources\Other\ModernKeePass\ModernKeePass\TemplateSelectors\SelectableDataTemplateSelector.cs
	namespace 	
ModernKeePass
 
. 
TemplateSelectors )
{ 
public 

class *
SelectableDataTemplateSelector /
:/ 0 
DataTemplateSelector1 E
{ 
public		 
DataTemplate		 
TrueItem		 $
{		% &
get		' *
;		* +
set		, /
;		/ 0
}		1 2
public

 
DataTemplate

 
	FalseItem

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

2 3
	protected 
override 
DataTemplate '
SelectTemplateCore( :
(: ;
object; A
itemB F
,F G
DependencyObjectH X
	containerY b
)b c
{ 	
var 
isSelectableItem  
=! "
item# '
as( *
ISelectableModel+ ;
;; <
return 
isSelectableItem #
!=$ &
null' +
&&, .
isSelectableItem/ ?
.? @

IsSelected@ J
?K L
TrueItemM U
:V W
	FalseItemX a
;a b
} 	
} 
} ¯

OC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\Items\SettingsSaveVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
SettingsSaveVm 
{ 
private 
readonly 
ISettingsService )
	_settings* 3
;3 4
public

 
SettingsSaveVm

 
(

 
)

 
:

  !
this

" &
(

& '
SettingsService

' 6
.

6 7
Instance

7 ?
)

? @
{ 	
}
 
public 
SettingsSaveVm 
( 
ISettingsService .
settings/ 7
)7 8
{ 	
	_settings 
= 
settings  
;  !
} 	
public 
bool 
IsSaveSuspend !
{ 	
get 
{ 
return 
	_settings "
." #

GetSetting# -
(- .
$str. ;
,; <
true= A
)A B
;B C
}D E
set 
{ 
	_settings 
. 

PutSetting &
(& '
$str' 4
,4 5
value6 ;
); <
;< =
}> ?
} 	
} 
} ˛
TC:\Sources\Other\ModernKeePass\ModernKeePass\Views\MainPageFrames\DonatePage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 

DonatePage  *
{		 
public

 

DonatePage

 
(

 
)

 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} ª¨
HC:\Sources\Other\ModernKeePass\ModernKeePass\Services\DatabaseService.cs
	namespace 	
ModernKeePass
 
. 
Services  
{ 
public 

class 
DatabaseService  
:  ! 
SingletonServiceBase" 6
<6 7
DatabaseService7 F
>F G
,G H
IDatabaseServiceI Y
{ 
private 
readonly 

PwDatabase #
_pwDatabase$ /
=0 1
new2 5

PwDatabase6 @
(@ A
)A B
;B C
private 
readonly 
ISettingsService )
	_settings* 3
;3 4
private 
StorageFile 
_realDatabaseFile -
;- .
private 
StorageFile 
_databaseFile )
;) *
private 
GroupVm 
_recycleBin #
;# $
private 
CompositeKey 
_compositeKey *
;* +
public 
GroupVm 
	RootGroup  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
GroupVm 

RecycleBin !
{ 	
get 
{ 
return 
_recycleBin $
;$ %
}& '
set   
{!! 
_recycleBin"" 
="" 
value"" #
;""# $
_pwDatabase## 
.## 
RecycleBinUuid## *
=##+ ,
_recycleBin##- 8
?##8 9
.##9 :
IdUuid##: @
;##@ A
}$$ 
}%% 	
public'' 
string'' 
Name'' 
=>'' 
DatabaseFile'' *
?''* +
.''+ ,
Name'', 0
;''0 1
public)) 
bool)) 
RecycleBinEnabled)) %
{** 	
get++ 
{++ 
return++ 
_pwDatabase++ $
.++$ %
RecycleBinEnabled++% 6
;++6 7
}++8 9
set,, 
{,, 
_pwDatabase,, 
.,, 
RecycleBinEnabled,, /
=,,0 1
value,,2 7
;,,7 8
},,9 :
}-- 	
public// 
StorageFile// 
DatabaseFile// '
{00 	
get11 
{11 
return11 
_databaseFile11 &
;11& '
}11( )
set22 
{33 
if44 
(44 
IsOpen44 
&&44 

HasChanged44 (
)44( )
{55 
throw66 
new66 #
DatabaseOpenedException66 5
(665 6
)666 7
;667 8
}77 
_databaseFile88 
=88 
value88  %
;88% &
}99 
}:: 	
public<< 
CompositeKey<< 
CompositeKey<< (
{== 	
get>> 
{>> 
return>> 
_compositeKey>> &
;>>& '
}>>( )
set?? 
{?? 
_compositeKey?? 
=??  !
value??" '
;??' (
}??) *
}@@ 	
publicBB 
PwUuidBB 

DataCipherBB  
{CC 	
getDD 
{DD 
returnDD 
_pwDatabaseDD $
.DD$ %
DataCipherUuidDD% 3
;DD3 4
}DD5 6
setEE 
{EE 
_pwDatabaseEE 
.EE 
DataCipherUuidEE ,
=EE- .
valueEE/ 4
;EE4 5
}EE6 7
}FF 	
publicHH "
PwCompressionAlgorithmHH % 
CompressionAlgorithmHH& :
{II 	
getJJ 
{JJ 
returnJJ 
_pwDatabaseJJ $
.JJ$ %
CompressionJJ% 0
;JJ0 1
}JJ2 3
setKK 
{KK 
_pwDatabaseKK 
.KK 
CompressionKK )
=KK* +
valueKK, 1
;KK1 2
}KK3 4
}LL 	
publicNN 
KdfParametersNN 
KeyDerivationNN *
{OO 	
getPP 
{PP 
returnPP 
_pwDatabasePP $
.PP$ %
KdfParametersPP% 2
;PP2 3
}PP4 5
setQQ 
{QQ 
_pwDatabaseQQ 
.QQ 
KdfParametersQQ +
=QQ, -
valueQQ. 3
;QQ3 4
}QQ5 6
}RR 	
publicTT 
boolTT 
IsOpenTT 
=>TT 
_pwDatabaseTT )
.TT) *
IsOpenTT* 0
;TT0 1
publicUU 
boolUU 

IsFileOpenUU 
=>UU !
!UU" #
_pwDatabaseUU# .
.UU. /
IsOpenUU/ 5
&&UU6 8
_databaseFileUU9 F
!=UUG I
nullUUJ N
;UUN O
publicVV 
boolVV 
IsClosedVV 
=>VV 
_databaseFileVV  -
==VV. 0
nullVV1 5
;VV5 6
publicWW 
boolWW 

HasChangedWW 
{WW  
getWW! $
;WW$ %
setWW& )
;WW) *
}WW+ ,
publicYY 
DatabaseServiceYY 
(YY 
)YY  
:YY! "
thisYY# '
(YY' (
SettingsServiceYY( 7
.YY7 8
InstanceYY8 @
)YY@ A
{ZZ 	
}[[ 	
public]] 
DatabaseService]] 
(]] 
ISettingsService]] /
settings]]0 8
)]]8 9
{^^ 	
	_settings__ 
=__ 
settings__  
;__  !
}`` 	
publicii 
asyncii 
Taskii 
Openii 
(ii 
CompositeKeyii +
keyii, /
,ii/ 0
boolii1 5
	createNewii6 ?
=ii@ A
falseiiB G
)iiG H
{jj 	
tryll 
{mm 
ifnn 
(nn 
keynn 
==nn 
nullnn 
)nn  
{oo 
throwpp 
newpp !
ArgumentNullExceptionpp 3
(pp3 4
nameofpp4 :
(pp: ;
keypp; >
)pp> ?
)pp? @
;pp@ A
}qq 
_compositeKeyss 
=ss 
keyss  #
;ss# $
vartt 
ioConnectiontt  
=tt! "
IOConnectionInfott# 3
.tt3 4
FromFilett4 <
(tt< =
DatabaseFilett= I
)ttI J
;ttJ K
ifuu 
(uu 
	createNewuu 
)uu 
{vv 
_pwDatabaseww 
.ww  
Newww  #
(ww# $
ioConnectionww$ 0
,ww0 1
keyww2 5
)ww5 6
;ww6 7
ifzz 
(zz 
	_settingszz !
.zz! "

GetSettingzz" ,
<zz, -
boolzz- 1
>zz1 2
(zz2 3
$strzz3 ;
)zz; <
)zz< =
CreateSampleDatazz> N
(zzN O
)zzO P
;zzP Q
var{{ 

fileFormat{{ "
={{# $
	_settings{{% .
.{{. /

GetSetting{{/ 9
<{{9 :
string{{: @
>{{@ A
({{A B
$str{{B U
){{U V
;{{V W
switch|| 
(|| 

fileFormat|| &
)||& '
{}} 
case~~ 
$str~~  
:~~  !
KeyDerivation )
=* +
KdfPool, 3
.3 4
Get4 7
(7 8
$str8 @
)@ A
.A B 
GetDefaultParametersB V
(V W
)W X
;X Y
break
ÄÄ !
;
ÄÄ! "
}
ÅÅ 
}
ÇÇ 
else
ÉÉ 
_pwDatabase
ÉÉ  
.
ÉÉ  !
Open
ÉÉ! %
(
ÉÉ% &
ioConnection
ÉÉ& 2
,
ÉÉ2 3
key
ÉÉ4 7
,
ÉÉ7 8
new
ÉÉ9 <
NullStatusLogger
ÉÉ= M
(
ÉÉM N
)
ÉÉN O
)
ÉÉO P
;
ÉÉP Q
if
àà 
(
àà 
	_settings
àà 
.
àà 

GetSetting
àà (
<
àà( )
bool
àà) -
>
àà- .
(
àà. /
$str
àà/ ?
)
àà? @
)
àà@ A
{
ââ 
_realDatabaseFile
ää %
=
ää& '
_databaseFile
ää( 5
;
ää5 6
var
ãã 

backupFile
ãã "
=
ãã# $
await
åå 
ApplicationData
åå -
.
åå- .
Current
åå. 5
.
åå5 6
RoamingFolder
åå6 C
.
ååC D
CreateFileAsync
ååD S
(
ååS T
Name
ååT X
,
ååX Y%
CreationCollisionOption
çç 3
.
çç3 4
FailIfExists
çç4 @
)
çç@ A
;
ççA B
Save
éé 
(
éé 

backupFile
éé #
)
éé# $
;
éé$ %
}
èè 
	RootGroup
ëë 
=
ëë 
new
ëë 
GroupVm
ëë  '
(
ëë' (
_pwDatabase
ëë( 3
.
ëë3 4
	RootGroup
ëë4 =
,
ëë= >
null
ëë? C
,
ëëC D
RecycleBinEnabled
ëëE V
?
ëëW X
_pwDatabase
ëëY d
.
ëëd e
RecycleBinUuid
ëëe s
:
ëët u
null
ëëv z
)
ëëz {
;
ëë{ |
}
íí 
catch
ìì 
(
ìì *
InvalidCompositeKeyException
ìì /
ex
ìì0 2
)
ìì2 3
{
îî 
HockeyClient
ïï 
.
ïï 
Current
ïï $
.
ïï$ %
TrackException
ïï% 3
(
ïï3 4
ex
ïï4 6
)
ïï6 7
;
ïï7 8
throw
ññ 
new
ññ 
ArgumentException
ññ +
(
ññ+ ,
ex
ññ, .
.
ññ. /
Message
ññ/ 6
,
ññ6 7
ex
ññ8 :
)
ññ: ;
;
ññ; <
}
óó 
}
òò 	
public
öö 
async
öö 
Task
öö 
ReOpen
öö  
(
öö  !
)
öö! "
{
õõ 	
await
úú 
Open
úú 
(
úú 
_compositeKey
úú $
)
úú$ %
;
úú% &
}
ùù 	
public
¢¢ 
void
¢¢ 
Save
¢¢ 
(
¢¢ 
)
¢¢ 
{
££ 	
if
§§ 
(
§§ 
!
§§ 
IsOpen
§§ 
)
§§ 
return
§§ 
;
§§  
try
•• 
{
¶¶ 
_pwDatabase
ßß 
.
ßß 
Save
ßß  
(
ßß  !
new
ßß! $
NullStatusLogger
ßß% 5
(
ßß5 6
)
ßß6 7
)
ßß7 8
;
ßß8 9
if
™™ 
(
™™ 
	_settings
™™ 
.
™™ 

GetSetting
™™ (
<
™™( )
bool
™™) -
>
™™- .
(
™™. /
$str
™™/ ?
)
™™? @
)
™™@ A
{
´´ 
_pwDatabase
¨¨ 
.
¨¨  
Open
¨¨  $
(
¨¨$ %
_pwDatabase
¨¨% 0
.
¨¨0 1
IOConnectionInfo
¨¨1 A
,
¨¨A B
_pwDatabase
¨¨C N
.
¨¨N O
	MasterKey
¨¨O X
,
¨¨X Y
new
¨¨Z ]
NullStatusLogger
¨¨^ n
(
¨¨n o
)
¨¨o p
)
¨¨p q
;
¨¨q r
}
≠≠ 
}
ÆÆ 
catch
ØØ 
(
ØØ 
	Exception
ØØ 
e
ØØ 
)
ØØ 
{
∞∞ 
throw
±± 
new
±± 
SaveException
±± '
(
±±' (
e
±±( )
)
±±) *
;
±±* +
}
≤≤ 
}
≥≥ 	
public
ππ 
void
ππ 
Save
ππ 
(
ππ 
StorageFile
ππ $
file
ππ% )
)
ππ) *
{
∫∫ 	
var
ªª 
oldFile
ªª 
=
ªª 
DatabaseFile
ªª &
;
ªª& '
DatabaseFile
ºº 
=
ºº 
file
ºº 
;
ºº  
try
ΩΩ 
{
ææ 
_pwDatabase
øø 
.
øø 
SaveAs
øø "
(
øø" #
IOConnectionInfo
øø# 3
.
øø3 4
FromFile
øø4 <
(
øø< =
DatabaseFile
øø= I
)
øøI J
,
øøJ K
true
øøL P
,
øøP Q
new
øøR U
NullStatusLogger
øøV f
(
øøf g
)
øøg h
)
øøh i
;
øøi j
}
¿¿ 
catch
¡¡ 
{
¬¬ 
DatabaseFile
√√ 
=
√√ 
oldFile
√√ &
;
√√& '
throw
ƒƒ 
;
ƒƒ 
}
≈≈ 
}
∆∆ 	
public
ÀÀ 
async
ÀÀ 
Task
ÀÀ 
Close
ÀÀ 
(
ÀÀ  
bool
ÀÀ  $
releaseFile
ÀÀ% 0
=
ÀÀ1 2
true
ÀÀ3 7
)
ÀÀ7 8
{
ÃÃ 	
_pwDatabase
ÕÕ 
?
ÕÕ 
.
ÕÕ 
Close
ÕÕ 
(
ÕÕ 
)
ÕÕ  
;
ÕÕ  !
if
–– 
(
–– 
	_settings
–– 
.
–– 

GetSetting
–– $
<
––$ %
bool
––% )
>
––) *
(
––* +
$str
––+ ;
)
––; <
)
––< =
{
—— 
if
““ 
(
““ 
_pwDatabase
““ 
!=
““  "
null
““# '
&&
““( *
_pwDatabase
““+ 6
.
““6 7
Modified
““7 ?
)
““? @
Save
”” 
(
”” 
_realDatabaseFile
”” *
)
””* +
;
””+ ,
await
‘‘ 
DatabaseFile
‘‘ "
.
‘‘" #
DeleteAsync
‘‘# .
(
‘‘. /
)
‘‘/ 0
;
‘‘0 1
}
’’ 
if
÷÷ 
(
÷÷ 
releaseFile
÷÷ 
)
÷÷ 
DatabaseFile
÷÷ )
=
÷÷* +
null
÷÷, 0
;
÷÷0 1
}
◊◊ 	
public
ŸŸ 
void
ŸŸ 
AddDeletedItem
ŸŸ "
(
ŸŸ" #
PwUuid
ŸŸ# )
id
ŸŸ* ,
)
ŸŸ, -
{
⁄⁄ 	
_pwDatabase
€€ 
.
€€ 
DeletedObjects
€€ &
.
€€& '
Add
€€' *
(
€€* +
new
€€+ .
PwDeletedObject
€€/ >
(
€€> ?
id
€€? A
,
€€A B
DateTime
€€C K
.
€€K L
UtcNow
€€L R
)
€€R S
)
€€S T
;
€€T U
}
‹‹ 	
public
ﬁﬁ 
void
ﬁﬁ 
CreateRecycleBin
ﬁﬁ $
(
ﬁﬁ$ %
string
ﬁﬁ% +
title
ﬁﬁ, 1
)
ﬁﬁ1 2
{
ﬂﬂ 	

RecycleBin
‡‡ 
=
‡‡ 
	RootGroup
‡‡ "
.
‡‡" #
AddNewGroup
‡‡# .
(
‡‡. /
title
‡‡/ 4
)
‡‡4 5
;
‡‡5 6

RecycleBin
·· 
.
·· 

IsSelected
·· !
=
··" #
true
··$ (
;
··( )

RecycleBin
‚‚ 
.
‚‚ 
IconId
‚‚ 
=
‚‚ 
(
‚‚  !
int
‚‚! $
)
‚‚$ %
PwIcon
‚‚% +
.
‚‚+ ,
TrashBin
‚‚, 4
;
‚‚4 5
}
„„ 	
private
ÂÂ 
void
ÂÂ 
CreateSampleData
ÂÂ %
(
ÂÂ% &
)
ÂÂ& '
{
ÊÊ 	
_pwDatabase
ÁÁ 
.
ÁÁ 
	RootGroup
ÁÁ !
.
ÁÁ! "
AddGroup
ÁÁ" *
(
ÁÁ* +
new
ÁÁ+ .
PwGroup
ÁÁ/ 6
(
ÁÁ6 7
true
ÁÁ7 ;
,
ÁÁ; <
true
ÁÁ= A
,
ÁÁA B
$str
ÁÁC L
,
ÁÁL M
PwIcon
ÁÁN T
.
ÁÁT U
Count
ÁÁU Z
)
ÁÁZ [
,
ÁÁ[ \
true
ÁÁ] a
)
ÁÁa b
;
ÁÁb c
_pwDatabase
ËË 
.
ËË 
	RootGroup
ËË !
.
ËË! "
AddGroup
ËË" *
(
ËË* +
new
ËË+ .
PwGroup
ËË/ 6
(
ËË6 7
true
ËË7 ;
,
ËË; <
true
ËË= A
,
ËËA B
$str
ËËC J
,
ËËJ K
PwIcon
ËËL R
.
ËËR S
EMail
ËËS X
)
ËËX Y
,
ËËY Z
true
ËË[ _
)
ËË_ `
;
ËË` a
_pwDatabase
ÈÈ 
.
ÈÈ 
	RootGroup
ÈÈ !
.
ÈÈ! "
AddGroup
ÈÈ" *
(
ÈÈ* +
new
ÈÈ+ .
PwGroup
ÈÈ/ 6
(
ÈÈ6 7
true
ÈÈ7 ;
,
ÈÈ; <
true
ÈÈ= A
,
ÈÈA B
$str
ÈÈC M
,
ÈÈM N
PwIcon
ÈÈO U
.
ÈÈU V
World
ÈÈV [
)
ÈÈ[ \
,
ÈÈ\ ]
true
ÈÈ^ b
)
ÈÈb c
;
ÈÈc d
var
ÎÎ 
pe
ÎÎ 
=
ÎÎ 
new
ÎÎ 
PwEntry
ÎÎ  
(
ÎÎ  !
true
ÎÎ! %
,
ÎÎ% &
true
ÎÎ' +
)
ÎÎ+ ,
;
ÎÎ, -
pe
ÏÏ 
.
ÏÏ 
Strings
ÏÏ 
.
ÏÏ 
Set
ÏÏ 
(
ÏÏ 
PwDefs
ÏÏ !
.
ÏÏ! "

TitleField
ÏÏ" ,
,
ÏÏ, -
new
ÏÏ. 1
ProtectedString
ÏÏ2 A
(
ÏÏA B
_pwDatabase
ÏÏB M
.
ÏÏM N
MemoryProtection
ÏÏN ^
.
ÏÏ^ _
ProtectTitle
ÏÏ_ k
,
ÏÏk l
$str
ÌÌ 
)
ÌÌ 
)
ÌÌ  
;
ÌÌ  !
pe
ÓÓ 
.
ÓÓ 
Strings
ÓÓ 
.
ÓÓ 
Set
ÓÓ 
(
ÓÓ 
PwDefs
ÓÓ !
.
ÓÓ! "
UserNameField
ÓÓ" /
,
ÓÓ/ 0
new
ÓÓ1 4
ProtectedString
ÓÓ5 D
(
ÓÓD E
_pwDatabase
ÓÓE P
.
ÓÓP Q
MemoryProtection
ÓÓQ a
.
ÓÓa b
ProtectUserName
ÓÓb q
,
ÓÓq r
$str
ÔÔ 
)
ÔÔ 
)
ÔÔ 
;
ÔÔ 
pe
 
.
 
Strings
 
.
 
Set
 
(
 
PwDefs
 !
.
! "
UrlField
" *
,
* +
new
, /
ProtectedString
0 ?
(
? @
_pwDatabase
@ K
.
K L
MemoryProtection
L \
.
\ ]

ProtectUrl
] g
,
g h
PwDefs
ÒÒ 
.
ÒÒ 
HomepageUrl
ÒÒ "
)
ÒÒ" #
)
ÒÒ# $
;
ÒÒ$ %
pe
ÚÚ 
.
ÚÚ 
Strings
ÚÚ 
.
ÚÚ 
Set
ÚÚ 
(
ÚÚ 
PwDefs
ÚÚ !
.
ÚÚ! "
PasswordField
ÚÚ" /
,
ÚÚ/ 0
new
ÚÚ1 4
ProtectedString
ÚÚ5 D
(
ÚÚD E
_pwDatabase
ÚÚE P
.
ÚÚP Q
MemoryProtection
ÚÚQ a
.
ÚÚa b
ProtectPassword
ÚÚb q
,
ÚÚq r
$str
ÛÛ 
)
ÛÛ 
)
ÛÛ 
;
ÛÛ 
pe
ÙÙ 
.
ÙÙ 
Strings
ÙÙ 
.
ÙÙ 
Set
ÙÙ 
(
ÙÙ 
PwDefs
ÙÙ !
.
ÙÙ! "

NotesField
ÙÙ" ,
,
ÙÙ, -
new
ÙÙ. 1
ProtectedString
ÙÙ2 A
(
ÙÙA B
_pwDatabase
ÙÙB M
.
ÙÙM N
MemoryProtection
ÙÙN ^
.
ÙÙ^ _
ProtectNotes
ÙÙ_ k
,
ÙÙk l
$str
ıı 3
)
ıı3 4
)
ıı4 5
;
ıı5 6
_pwDatabase
ˆˆ 
.
ˆˆ 
	RootGroup
ˆˆ !
.
ˆˆ! "
AddEntry
ˆˆ" *
(
ˆˆ* +
pe
ˆˆ+ -
,
ˆˆ- .
true
ˆˆ/ 3
)
ˆˆ3 4
;
ˆˆ4 5
pe
¯¯ 
=
¯¯ 
new
¯¯ 
PwEntry
¯¯ 
(
¯¯ 
true
¯¯ !
,
¯¯! "
true
¯¯# '
)
¯¯' (
;
¯¯( )
pe
˘˘ 
.
˘˘ 
Strings
˘˘ 
.
˘˘ 
Set
˘˘ 
(
˘˘ 
PwDefs
˘˘ !
.
˘˘! "

TitleField
˘˘" ,
,
˘˘, -
new
˘˘. 1
ProtectedString
˘˘2 A
(
˘˘A B
_pwDatabase
˘˘B M
.
˘˘M N
MemoryProtection
˘˘N ^
.
˘˘^ _
ProtectTitle
˘˘_ k
,
˘˘k l
$str
˙˙ !
)
˙˙! "
)
˙˙" #
;
˙˙# $
pe
˚˚ 
.
˚˚ 
Strings
˚˚ 
.
˚˚ 
Set
˚˚ 
(
˚˚ 
PwDefs
˚˚ !
.
˚˚! "
UserNameField
˚˚" /
,
˚˚/ 0
new
˚˚1 4
ProtectedString
˚˚5 D
(
˚˚D E
_pwDatabase
˚˚E P
.
˚˚P Q
MemoryProtection
˚˚Q a
.
˚˚a b
ProtectUserName
˚˚b q
,
˚˚q r
$str
¸¸ 
)
¸¸ 
)
¸¸ 
;
¸¸ 
pe
˝˝ 
.
˝˝ 
Strings
˝˝ 
.
˝˝ 
Set
˝˝ 
(
˝˝ 
PwDefs
˝˝ !
.
˝˝! "
UrlField
˝˝" *
,
˝˝* +
new
˝˝, /
ProtectedString
˝˝0 ?
(
˝˝? @
_pwDatabase
˝˝@ K
.
˝˝K L
MemoryProtection
˝˝L \
.
˝˝\ ]

ProtectUrl
˝˝] g
,
˝˝g h
PwDefs
˛˛ 
.
˛˛ 
HelpUrl
˛˛ 
+
˛˛  
$str
˛˛! 3
)
˛˛3 4
)
˛˛4 5
;
˛˛5 6
pe
ˇˇ 
.
ˇˇ 
Strings
ˇˇ 
.
ˇˇ 
Set
ˇˇ 
(
ˇˇ 
PwDefs
ˇˇ !
.
ˇˇ! "
PasswordField
ˇˇ" /
,
ˇˇ/ 0
new
ˇˇ1 4
ProtectedString
ˇˇ5 D
(
ˇˇD E
_pwDatabase
ˇˇE P
.
ˇˇP Q
MemoryProtection
ˇˇQ a
.
ˇˇa b
ProtectPassword
ˇˇb q
,
ˇˇq r
$str
ÄÄ 
)
ÄÄ 
)
ÄÄ 
;
ÄÄ 
pe
ÅÅ 
.
ÅÅ 
AutoType
ÅÅ 
.
ÅÅ 
Add
ÅÅ 
(
ÅÅ 
new
ÅÅ !
AutoTypeAssociation
ÅÅ  3
(
ÅÅ3 4
$str
ÅÅ4 K
,
ÅÅK L
string
ÅÅM S
.
ÅÅS T
Empty
ÅÅT Y
)
ÅÅY Z
)
ÅÅZ [
;
ÅÅ[ \
_pwDatabase
ÇÇ 
.
ÇÇ 
	RootGroup
ÇÇ !
.
ÇÇ! "
AddEntry
ÇÇ" *
(
ÇÇ* +
pe
ÇÇ+ -
,
ÇÇ- .
true
ÇÇ/ 3
)
ÇÇ3 4
;
ÇÇ4 5
}
ÉÉ 	
}
ÑÑ 
}ÖÖ ∏
KC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\ISettingsService.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface 
ISettingsService %
{ 
T 	

GetSetting
 
< 
T 
> 
( 
string 
property '
,' (
T) *
defaultValue+ 7
=8 9
default: A
(A B
TB C
)C D
)D E
;E F
void 

PutSetting 
< 
T 
> 
( 
string !
property" *
,* +
T, -
value. 3
)3 4
;4 5
} 
} ∑8
JC:\Sources\Other\ModernKeePass\ModernKeePass\Common\MessageDialogHelper.cs
	namespace 	
ModernKeePass
 
. 
Common 
{		 
public

 

static

 
class

 
MessageDialogHelper

 +
{ 
public 
static 
async 
void  
ShowActionDialog! 1
(1 2
string2 8
title9 >
,> ?
string@ F
contentTextG R
,R S
stringT Z
actionButtonText[ k
,k l
stringm s
cancelButtonText	t Ñ
,
Ñ Ö%
UICommandInvokedHandler
Ü ù
actionCommand
û ´
,
´ ¨%
UICommandInvokedHandler
≠ ƒ
cancelCommand
≈ “
)
“ ”
{ 	
var 
messageDialog 
= 
CreateBasicDialog  1
(1 2
title2 7
,7 8
contentText9 D
,D E
cancelButtonTextF V
,V W
cancelCommandX e
)e f
;f g
messageDialog 
. 
Commands "
." #
Add# &
(& '
new' *
	UICommand+ 4
(4 5
actionButtonText5 E
,E F
actionCommandG T
)T U
)U V
;V W
await 
messageDialog 
.  
	ShowAsync  )
() *
)* +
;+ ,
} 	
public 
static 
void 
SaveErrorDialog *
(* +
SaveException+ 8
	exception9 B
,B C
IDatabaseServiceD T
databaseU ]
)] ^
{ 	
ShowActionDialog 
( 
$str )
,) *
	exception+ 4
.4 5
InnerException5 C
.C D
MessageD K
,K L
$strM V
,V W
$strX a
,a b
asyncc h
commandi p
=>q s
{ 
var 

savePicker 
=  
new! $
FileSavePicker% 3
{ "
SuggestedStartLocation *
=+ ,
PickerLocationId- =
.= >
DocumentsLibrary> N
,N O
SuggestedFileName   %
=  & '
$"  ( *
{  * +
database  + 3
.  3 4
DatabaseFile  4 @
.  @ A
DisplayName  A L
}  L M
 - copy  M T
"  T U
}!! 
;!! 

savePicker"" 
."" 
FileTypeChoices"" *
.""* +
Add""+ .
("". /
$str""/ E
,""E F
new""G J
List""K O
<""O P
string""P V
>""V W
{""X Y
$str""Z a
}""b c
)""c d
;""d e
var$$ 
file$$ 
=$$ 
await$$  

savePicker$$! +
.$$+ ,
PickSaveFileAsync$$, =
($$= >
)$$> ?
;$$? @
if%% 
(%% 
file%% 
!=%% 
null%%  
)%%  !
database%%" *
.%%* +
Save%%+ /
(%%/ 0
file%%0 4
)%%4 5
;%%5 6
}&& 
,&& 
null&& 
)&& 
;&& 
}'' 	
public)) 
static)) 
void)) 
SaveUnchangedDialog)) .
()). /#
DatabaseOpenedException))/ F
	exception))G P
,))P Q
IDatabaseService))R b
database))c k
)))k l
{** 	
ShowActionDialog++ 
(++ 
$str++ .
,++. /
$"++0 2
	Database ++2 ;
{++; <
database++< D
.++D E
Name++E I
}++I J9
- is currently opened. What to you wish to do?++J w
"++w x
,++x y
$str	++z à
,
++à â
$str
++ä ì
,
++ì î
command
++ï ú
=>
++ù ü
{,, 
database-- 
.-- 
Save-- 
(-- 
)-- 
;--  
database.. 
... 
Close.. 
(.. 
)..  
;..  !
}// 
,// 
command00 
=>00 
{11 
database22 
.22 
Close22 
(22 
)22  
;22  !
}33 
)33 
;33 
}44 	
public66 
static66 
async66 
void66  
ShowErrorDialog66! 0
(660 1
	Exception661 :
	exception66; D
)66D E
{77 	
if88 
(88 
	exception88 
==88 
null88 !
)88! "
return88# )
;88) *
var:: 
messageDialog:: 
=:: 
CreateBasicDialog::  1
(::1 2
	exception::2 ;
.::; <
Message::< C
,::C D
	exception::E N
.::N O

StackTrace::O Y
,::Y Z
$str::[ _
)::_ `
;::` a
await== 
messageDialog== 
.==  
	ShowAsync==  )
(==) *
)==* +
;==+ ,
}>> 	
public@@ 
static@@ 
async@@ 
void@@  "
ShowNotificationDialog@@! 7
(@@7 8
string@@8 >
title@@? D
,@@D E
string@@F L
message@@M T
)@@T U
{AA 	
varBB 
dialogBB 
=BB 
CreateBasicDialogBB *
(BB* +
titleBB+ 0
,BB0 1
messageBB2 9
,BB9 :
$strBB; ?
)BB? @
;BB@ A
awaitEE 
dialogEE 
.EE 
	ShowAsyncEE "
(EE" #
)EE# $
;EE$ %
}FF 	
privateHH 
staticHH 
MessageDialogHH $
CreateBasicDialogHH% 6
(HH6 7
stringHH7 =
titleHH> C
,HHC D
stringHHE K
messageHHL S
,HHS T
stringHHU [
dismissActionTextHH\ m
,HHm n$
UICommandInvokedHandler	HHo Ü
cancelCommand
HHá î
=
HHï ñ
null
HHó õ
)
HHõ ú
{II 	
varKK 
messageDialogKK 
=KK 
newKK  #
MessageDialogKK$ 1
(KK1 2
messageKK2 9
,KK9 :
titleKK; @
)KK@ A
;KKA B
messageDialogNN 
.NN 
CommandsNN "
.NN" #
AddNN# &
(NN& '
newNN' *
	UICommandNN+ 4
(NN4 5
dismissActionTextNN5 F
,NNF G
cancelCommandNNH U
)NNU V
)NNV W
;NNW X
messageDialogQQ 
.QQ 
DefaultCommandIndexQQ -
=QQ. /
$numQQ0 1
;QQ1 2
messageDialogTT 
.TT 
CancelCommandIndexTT ,
=TT- .
$numTT/ 0
;TT0 1
returnVV 
messageDialogVV  
;VV  !
}WW 	
}XX 
}YY éß
GC:\Sources\Other\ModernKeePass\ModernKeePass\Common\NavigationHelper.cs
	namespace 	
ModernKeePass
 
. 
Common 
{ 
[;; 
Windows;; 
.;; 

Foundation;; 
.;; 
Metadata;;  
.;;  !
WebHostHidden;;! .
];;. /
public<< 

class<< 
NavigationHelper<< !
:<<" #
DependencyObject<<$ 4
{== 
private>> 
Page>> 
Page>> 
{>> 
get>> 
;>>  
set>>! $
;>>$ %
}>>& '
private?? 
Frame?? 
Frame?? 
{?? 
get?? !
{??" #
return??$ *
this??+ /
.??/ 0
Page??0 4
.??4 5
Frame??5 :
;??: ;
}??< =
}??> ?
publicGG 
NavigationHelperGG 
(GG  
PageGG  $
pageGG% )
)GG) *
{HH 	
thisII 
.II 
PageII 
=II 
pageII 
;II 
thisNN 
.NN 
PageNN 
.NN 
LoadedNN 
+=NN 
(NN  !
senderNN! '
,NN' (
eNN) *
)NN* +
=>NN, .
{OO 
ifTT 
(TT 
thisTT 
.TT 
PageTT 
.TT 
ActualHeightTT *
==TT+ -
WindowTT. 4
.TT4 5
CurrentTT5 <
.TT< =
BoundsTT= C
.TTC D
HeightTTD J
&&TTK M
thisUU 
.UU 
PageUU 
.UU 
ActualWidthUU )
==UU* ,
WindowUU- 3
.UU3 4
CurrentUU4 ;
.UU; <
BoundsUU< B
.UUB C
WidthUUC H
)UUH I
{VV 
WindowXX 
.XX 
CurrentXX "
.XX" #

CoreWindowXX# -
.XX- .

DispatcherXX. 8
.XX8 9#
AcceleratorKeyActivatedXX9 P
+=XXQ S2
&CoreDispatcher_AcceleratorKeyActivatedYY >
;YY> ?
WindowZZ 
.ZZ 
CurrentZZ "
.ZZ" #

CoreWindowZZ# -
.ZZ- .
PointerPressedZZ. <
+=ZZ= ?
this[[ 
.[[ %
CoreWindow_PointerPressed[[ 6
;[[6 7
}\\ 
}^^ 
;^^ 
thisaa 
.aa 
Pageaa 
.aa 
Unloadedaa 
+=aa !
(aa" #
senderaa# )
,aa) *
eaa+ ,
)aa, -
=>aa. 0
{bb 
Windowff 
.ff 
Currentff 
.ff 

CoreWindowff )
.ff) *

Dispatcherff* 4
.ff4 5#
AcceleratorKeyActivatedff5 L
-=ffM O2
&CoreDispatcher_AcceleratorKeyActivatedgg :
;gg: ;
Windowhh 
.hh 
Currenthh 
.hh 

CoreWindowhh )
.hh) *
PointerPressedhh* 8
-=hh9 ;
thisii 
.ii %
CoreWindow_PointerPressedii 2
;ii2 3
}kk 
;kk 
}ll 	
RelayCommandpp 
_goBackCommandpp #
;pp# $
RelayCommandqq 
_goForwardCommandqq &
;qq& '
public{{ 
RelayCommand{{ 
GoBackCommand{{ )
{|| 	
get}} 
{~~ 
if 
( 
_goBackCommand "
==# %
null& *
)* +
{
ÄÄ 
_goBackCommand
ÅÅ "
=
ÅÅ# $
new
ÅÅ% (
RelayCommand
ÅÅ) 5
(
ÅÅ5 6
(
ÇÇ 
)
ÇÇ 
=>
ÇÇ 
this
ÇÇ "
.
ÇÇ" #
GoBack
ÇÇ# )
(
ÇÇ) *
)
ÇÇ* +
,
ÇÇ+ ,
(
ÉÉ 
)
ÉÉ 
=>
ÉÉ 
this
ÉÉ "
.
ÉÉ" #
	CanGoBack
ÉÉ# ,
(
ÉÉ, -
)
ÉÉ- .
)
ÉÉ. /
;
ÉÉ/ 0
}
ÑÑ 
return
ÖÖ 
_goBackCommand
ÖÖ %
;
ÖÖ% &
}
ÜÜ 
set
áá 
{
àà 
_goBackCommand
ââ 
=
ââ  
value
ââ! &
;
ââ& '
}
ää 
}
ãã 	
public
ìì 
RelayCommand
ìì 
GoForwardCommand
ìì ,
{
îî 	
get
ïï 
{
ññ 
if
óó 
(
óó 
_goForwardCommand
óó %
==
óó& (
null
óó) -
)
óó- .
{
òò 
_goForwardCommand
ôô %
=
ôô& '
new
ôô( +
RelayCommand
ôô, 8
(
ôô8 9
(
öö 
)
öö 
=>
öö 
this
öö "
.
öö" #
	GoForward
öö# ,
(
öö, -
)
öö- .
,
öö. /
(
õõ 
)
õõ 
=>
õõ 
this
õõ "
.
õõ" #
CanGoForward
õõ# /
(
õõ/ 0
)
õõ0 1
)
õõ1 2
;
õõ2 3
}
úú 
return
ùù 
_goForwardCommand
ùù (
;
ùù( )
}
ûû 
}
üü 	
public
©© 
virtual
©© 
bool
©© 
	CanGoBack
©© %
(
©©% &
)
©©& '
{
™™ 	
return
´´ 
this
´´ 
.
´´ 
Frame
´´ 
!=
´´  
null
´´! %
&&
´´& (
this
´´) -
.
´´- .
Frame
´´. 3
.
´´3 4
	CanGoBack
´´4 =
;
´´= >
}
¨¨ 	
public
µµ 
virtual
µµ 
bool
µµ 
CanGoForward
µµ (
(
µµ( )
)
µµ) *
{
∂∂ 	
return
∑∑ 
this
∑∑ 
.
∑∑ 
Frame
∑∑ 
!=
∑∑  
null
∑∑! %
&&
∑∑& (
this
∑∑) -
.
∑∑- .
Frame
∑∑. 3
.
∑∑3 4
CanGoForward
∑∑4 @
;
∑∑@ A
}
∏∏ 	
public
ææ 
virtual
ææ 
void
ææ 
GoBack
ææ "
(
ææ" #
)
ææ# $
{
øø 	
if
¿¿ 
(
¿¿ 
this
¿¿ 
.
¿¿ 
Frame
¿¿ 
!=
¿¿ 
null
¿¿ "
&&
¿¿# %
this
¿¿& *
.
¿¿* +
Frame
¿¿+ 0
.
¿¿0 1
	CanGoBack
¿¿1 :
)
¿¿: ;
this
¿¿< @
.
¿¿@ A
Frame
¿¿A F
.
¿¿F G
GoBack
¿¿G M
(
¿¿M N
)
¿¿N O
;
¿¿O P
}
¡¡ 	
public
∆∆ 
virtual
∆∆ 
void
∆∆ 
	GoForward
∆∆ %
(
∆∆% &
)
∆∆& '
{
«« 	
if
»» 
(
»» 
this
»» 
.
»» 
Frame
»» 
!=
»» 
null
»» "
&&
»»# %
this
»»& *
.
»»* +
Frame
»»+ 0
.
»»0 1
CanGoForward
»»1 =
)
»»= >
this
»»? C
.
»»C D
Frame
»»D I
.
»»I J
	GoForward
»»J S
(
»»S T
)
»»T U
;
»»U V
}
…… 	
private
·· 
void
·· 4
&CoreDispatcher_AcceleratorKeyActivated
·· ;
(
··; <
CoreDispatcher
··< J
sender
··K Q
,
··Q R%
AcceleratorKeyEventArgs
‚‚ #
e
‚‚$ %
)
‚‚% &
{
„„ 	
var
‰‰ 

virtualKey
‰‰ 
=
‰‰ 
e
‰‰ 
.
‰‰ 

VirtualKey
‰‰ )
;
‰‰) *
if
ËË 
(
ËË 
(
ËË 
e
ËË 
.
ËË 
	EventType
ËË 
==
ËË )
CoreAcceleratorKeyEventType
ËË  ;
.
ËË; <
SystemKeyDown
ËË< I
||
ËËJ L
e
ÈÈ 
.
ÈÈ 
	EventType
ÈÈ 
==
ÈÈ )
CoreAcceleratorKeyEventType
ÈÈ :
.
ÈÈ: ;
KeyDown
ÈÈ; B
)
ÈÈB C
&&
ÈÈD F
(
ÍÍ 

virtualKey
ÍÍ 
==
ÍÍ 

VirtualKey
ÍÍ )
.
ÍÍ) *
Left
ÍÍ* .
||
ÍÍ/ 1

virtualKey
ÍÍ2 <
==
ÍÍ= ?

VirtualKey
ÍÍ@ J
.
ÍÍJ K
Right
ÍÍK P
||
ÍÍQ S
(
ÎÎ 
int
ÎÎ 
)
ÎÎ 

virtualKey
ÎÎ 
==
ÎÎ  "
$num
ÎÎ# &
||
ÎÎ' )
(
ÎÎ* +
int
ÎÎ+ .
)
ÎÎ. /

virtualKey
ÎÎ/ 9
==
ÎÎ: <
$num
ÎÎ= @
)
ÎÎ@ A
)
ÎÎA B
{
ÏÏ 
var
ÌÌ 

coreWindow
ÌÌ 
=
ÌÌ  
Window
ÌÌ! '
.
ÌÌ' (
Current
ÌÌ( /
.
ÌÌ/ 0

CoreWindow
ÌÌ0 :
;
ÌÌ: ;
var
ÓÓ 
	downState
ÓÓ 
=
ÓÓ "
CoreVirtualKeyStates
ÓÓ  4
.
ÓÓ4 5
Down
ÓÓ5 9
;
ÓÓ9 :
bool
ÔÔ 
menuKey
ÔÔ 
=
ÔÔ 
(
ÔÔ  

coreWindow
ÔÔ  *
.
ÔÔ* +
GetKeyState
ÔÔ+ 6
(
ÔÔ6 7

VirtualKey
ÔÔ7 A
.
ÔÔA B
Menu
ÔÔB F
)
ÔÔF G
&
ÔÔH I
	downState
ÔÔJ S
)
ÔÔS T
==
ÔÔU W
	downState
ÔÔX a
;
ÔÔa b
bool
 

controlKey
 
=
  !
(
" #

coreWindow
# -
.
- .
GetKeyState
. 9
(
9 :

VirtualKey
: D
.
D E
Control
E L
)
L M
&
N O
	downState
P Y
)
Y Z
==
[ ]
	downState
^ g
;
g h
bool
ÒÒ 
shiftKey
ÒÒ 
=
ÒÒ 
(
ÒÒ  !

coreWindow
ÒÒ! +
.
ÒÒ+ ,
GetKeyState
ÒÒ, 7
(
ÒÒ7 8

VirtualKey
ÒÒ8 B
.
ÒÒB C
Shift
ÒÒC H
)
ÒÒH I
&
ÒÒJ K
	downState
ÒÒL U
)
ÒÒU V
==
ÒÒW Y
	downState
ÒÒZ c
;
ÒÒc d
bool
ÚÚ 
noModifiers
ÚÚ  
=
ÚÚ! "
!
ÚÚ# $
menuKey
ÚÚ$ +
&&
ÚÚ, .
!
ÚÚ/ 0

controlKey
ÚÚ0 :
&&
ÚÚ; =
!
ÚÚ> ?
shiftKey
ÚÚ? G
;
ÚÚG H
bool
ÛÛ 
onlyAlt
ÛÛ 
=
ÛÛ 
menuKey
ÛÛ &
&&
ÛÛ' )
!
ÛÛ* +

controlKey
ÛÛ+ 5
&&
ÛÛ6 8
!
ÛÛ9 :
shiftKey
ÛÛ: B
;
ÛÛB C
if
ıı 
(
ıı 
(
ıı 
(
ıı 
int
ıı 
)
ıı 

virtualKey
ıı $
==
ıı% '
$num
ıı( +
&&
ıı, .
noModifiers
ıı/ :
)
ıı: ;
||
ıı< >
(
ˆˆ 

virtualKey
ˆˆ 
==
ˆˆ  "

VirtualKey
ˆˆ# -
.
ˆˆ- .
Left
ˆˆ. 2
&&
ˆˆ3 5
onlyAlt
ˆˆ6 =
)
ˆˆ= >
)
ˆˆ> ?
{
˜˜ 
e
˘˘ 
.
˘˘ 
Handled
˘˘ 
=
˘˘ 
true
˘˘  $
;
˘˘$ %
this
˙˙ 
.
˙˙ 
GoBackCommand
˙˙ &
.
˙˙& '
Execute
˙˙' .
(
˙˙. /
null
˙˙/ 3
)
˙˙3 4
;
˙˙4 5
}
˚˚ 
else
¸¸ 
if
¸¸ 
(
¸¸ 
(
¸¸ 
(
¸¸ 
int
¸¸ 
)
¸¸ 

virtualKey
¸¸ )
==
¸¸* ,
$num
¸¸- 0
&&
¸¸1 3
noModifiers
¸¸4 ?
)
¸¸? @
||
¸¸A C
(
˝˝ 

virtualKey
˝˝ 
==
˝˝  "

VirtualKey
˝˝# -
.
˝˝- .
Right
˝˝. 3
&&
˝˝4 6
onlyAlt
˝˝7 >
)
˝˝> ?
)
˝˝? @
{
˛˛ 
e
ÄÄ 
.
ÄÄ 
Handled
ÄÄ 
=
ÄÄ 
true
ÄÄ  $
;
ÄÄ$ %
this
ÅÅ 
.
ÅÅ 
GoForwardCommand
ÅÅ )
.
ÅÅ) *
Execute
ÅÅ* 1
(
ÅÅ1 2
null
ÅÅ2 6
)
ÅÅ6 7
;
ÅÅ7 8
}
ÇÇ 
}
ÉÉ 
}
ÑÑ 	
private
çç 
void
çç '
CoreWindow_PointerPressed
çç .
(
çç. /

CoreWindow
çç/ 9
sender
çç: @
,
çç@ A
PointerEventArgs
éé 
e
éé 
)
éé 
{
èè 	
var
êê 

properties
êê 
=
êê 
e
êê 
.
êê 
CurrentPoint
êê +
.
êê+ ,

Properties
êê, 6
;
êê6 7
if
ìì 
(
ìì 

properties
ìì 
.
ìì !
IsLeftButtonPressed
ìì .
||
ìì/ 1

properties
ìì2 <
.
ìì< ="
IsRightButtonPressed
ìì= Q
||
ììR T

properties
îî 
.
îî #
IsMiddleButtonPressed
îî 0
)
îî0 1
return
îî2 8
;
îî8 9
bool
óó 
backPressed
óó 
=
óó 

properties
óó )
.
óó) *
IsXButton1Pressed
óó* ;
;
óó; <
bool
òò 
forwardPressed
òò 
=
òò  !

properties
òò" ,
.
òò, -
IsXButton2Pressed
òò- >
;
òò> ?
if
ôô 
(
ôô 
backPressed
ôô 
^
ôô 
forwardPressed
ôô ,
)
ôô, -
{
öö 
e
õõ 
.
õõ 
Handled
õõ 
=
õõ 
true
õõ  
;
õõ  !
if
úú 
(
úú 
backPressed
úú 
)
úú  
this
úú! %
.
úú% &
GoBackCommand
úú& 3
.
úú3 4
Execute
úú4 ;
(
úú; <
null
úú< @
)
úú@ A
;
úúA B
if
ùù 
(
ùù 
forwardPressed
ùù "
)
ùù" #
this
ùù$ (
.
ùù( )
GoForwardCommand
ùù) 9
.
ùù9 :
Execute
ùù: A
(
ùùA B
null
ùùB F
)
ùùF G
;
ùùG H
}
ûû 
}
üü 	
private
¶¶ 
String
¶¶ 
_pageKey
¶¶ 
;
¶¶  
public
≠≠ 
event
≠≠ #
LoadStateEventHandler
≠≠ *
	LoadState
≠≠+ 4
;
≠≠4 5
public
¥¥ 
event
¥¥ #
SaveStateEventHandler
¥¥ *
	SaveState
¥¥+ 4
;
¥¥4 5
public
ΩΩ 
void
ΩΩ 
OnNavigatedTo
ΩΩ !
(
ΩΩ! "!
NavigationEventArgs
ΩΩ" 5
e
ΩΩ6 7
)
ΩΩ7 8
{
ææ 	
var
øø 

frameState
øø 
=
øø 
SuspensionManager
øø .
.
øø. /"
SessionStateForFrame
øø/ C
(
øøC D
this
øøD H
.
øøH I
Frame
øøI N
)
øøN O
;
øøO P
this
¿¿ 
.
¿¿ 
_pageKey
¿¿ 
=
¿¿ 
$str
¿¿ #
+
¿¿$ %
this
¿¿& *
.
¿¿* +
Frame
¿¿+ 0
.
¿¿0 1
BackStackDepth
¿¿1 ?
;
¿¿? @
if
¬¬ 
(
¬¬ 
e
¬¬ 
.
¬¬ 
NavigationMode
¬¬  
==
¬¬! #
NavigationMode
¬¬$ 2
.
¬¬2 3
New
¬¬3 6
)
¬¬6 7
{
√√ 
var
∆∆ 
nextPageKey
∆∆ 
=
∆∆  !
this
∆∆" &
.
∆∆& '
_pageKey
∆∆' /
;
∆∆/ 0
int
«« 
nextPageIndex
«« !
=
««" #
this
««$ (
.
««( )
Frame
««) .
.
««. /
BackStackDepth
««/ =
;
««= >
while
»» 
(
»» 

frameState
»» !
.
»»! "
Remove
»»" (
(
»»( )
nextPageKey
»») 4
)
»»4 5
)
»»5 6
{
…… 
nextPageIndex
   !
++
  ! #
;
  # $
nextPageKey
ÀÀ 
=
ÀÀ  !
$str
ÀÀ" )
+
ÀÀ* +
nextPageIndex
ÀÀ, 9
;
ÀÀ9 :
}
ÃÃ 
if
œœ 
(
œœ 
this
œœ 
.
œœ 
	LoadState
œœ "
!=
œœ# %
null
œœ& *
)
œœ* +
{
–– 
this
—— 
.
—— 
	LoadState
—— "
(
——" #
this
——# '
,
——' (
new
——) , 
LoadStateEventArgs
——- ?
(
——? @
e
——@ A
.
——A B
	Parameter
——B K
,
——K L
null
——M Q
)
——Q R
)
——R S
;
——S T
}
““ 
}
”” 
else
‘‘ 
{
’’ 
if
ŸŸ 
(
ŸŸ 
this
ŸŸ 
.
ŸŸ 
	LoadState
ŸŸ "
!=
ŸŸ# %
null
ŸŸ& *
)
ŸŸ* +
{
⁄⁄ 
this
€€ 
.
€€ 
	LoadState
€€ "
(
€€" #
this
€€# '
,
€€' (
new
€€) , 
LoadStateEventArgs
€€- ?
(
€€? @
e
€€@ A
.
€€A B
	Parameter
€€B K
,
€€K L
(
€€M N

Dictionary
€€N X
<
€€X Y
String
€€Y _
,
€€_ `
Object
€€a g
>
€€g h
)
€€h i

frameState
€€i s
[
€€s t
this
€€t x
.
€€x y
_pageKey€€y Å
]€€Å Ç
)€€Ç É
)€€É Ñ
;€€Ñ Ö
}
‹‹ 
}
›› 
}
ﬁﬁ 	
public
ÁÁ 
void
ÁÁ 
OnNavigatedFrom
ÁÁ #
(
ÁÁ# $!
NavigationEventArgs
ÁÁ$ 7
e
ÁÁ8 9
)
ÁÁ9 :
{
ËË 	
var
ÈÈ 

frameState
ÈÈ 
=
ÈÈ 
SuspensionManager
ÈÈ .
.
ÈÈ. /"
SessionStateForFrame
ÈÈ/ C
(
ÈÈC D
this
ÈÈD H
.
ÈÈH I
Frame
ÈÈI N
)
ÈÈN O
;
ÈÈO P
var
ÍÍ 
	pageState
ÍÍ 
=
ÍÍ 
new
ÍÍ 

Dictionary
ÍÍ  *
<
ÍÍ* +
String
ÍÍ+ 1
,
ÍÍ1 2
Object
ÍÍ3 9
>
ÍÍ9 :
(
ÍÍ: ;
)
ÍÍ; <
;
ÍÍ< =
if
ÎÎ 
(
ÎÎ 
this
ÎÎ 
.
ÎÎ 
	SaveState
ÎÎ 
!=
ÎÎ !
null
ÎÎ" &
)
ÎÎ& '
{
ÏÏ 
this
ÌÌ 
.
ÌÌ 
	SaveState
ÌÌ 
(
ÌÌ 
this
ÌÌ #
,
ÌÌ# $
new
ÌÌ% ( 
SaveStateEventArgs
ÌÌ) ;
(
ÌÌ; <
	pageState
ÌÌ< E
)
ÌÌE F
)
ÌÌF G
;
ÌÌG H
}
ÓÓ 

frameState
ÔÔ 
[
ÔÔ 
_pageKey
ÔÔ 
]
ÔÔ  
=
ÔÔ! "
	pageState
ÔÔ# ,
;
ÔÔ, -
}
 	
}
ÛÛ 
public
¯¯ 

delegate
¯¯ 
void
¯¯ #
LoadStateEventHandler
¯¯ .
(
¯¯. /
object
¯¯/ 5
sender
¯¯6 <
,
¯¯< = 
LoadStateEventArgs
¯¯> P
e
¯¯Q R
)
¯¯R S
;
¯¯S T
public
¸¸ 

delegate
¸¸ 
void
¸¸ #
SaveStateEventHandler
¸¸ .
(
¸¸. /
object
¸¸/ 5
sender
¸¸6 <
,
¸¸< = 
SaveStateEventArgs
¸¸> P
e
¸¸Q R
)
¸¸R S
;
¸¸S T
public
ÅÅ 

class
ÅÅ  
LoadStateEventArgs
ÅÅ #
:
ÅÅ$ %
	EventArgs
ÅÅ& /
{
ÇÇ 
public
áá 
Object
áá !
NavigationParameter
áá )
{
áá* +
get
áá, /
;
áá/ 0
private
áá1 8
set
áá9 <
;
áá< =
}
áá> ?
public
åå 

Dictionary
åå 
<
åå 
string
åå  
,
åå  !
Object
åå" (
>
åå( )
	PageState
åå* 3
{
åå4 5
get
åå6 9
;
åå9 :
private
åå; B
set
ååC F
;
ååF G
}
ååH I
public
ôô  
LoadStateEventArgs
ôô !
(
ôô! "
Object
ôô" (!
navigationParameter
ôô) <
,
ôô< =

Dictionary
ôô> H
<
ôôH I
string
ôôI O
,
ôôO P
Object
ôôQ W
>
ôôW X
	pageState
ôôY b
)
ôôb c
:
öö 
base
öö 
(
öö 
)
öö 
{
õõ 	
this
úú 
.
úú !
NavigationParameter
úú $
=
úú% &!
navigationParameter
úú' :
;
úú: ;
this
ùù 
.
ùù 
	PageState
ùù 
=
ùù 
	pageState
ùù &
;
ùù& '
}
ûû 	
}
üü 
public
££ 

class
££  
SaveStateEventArgs
££ #
:
££$ %
	EventArgs
££& /
{
§§ 
public
®® 

Dictionary
®® 
<
®® 
string
®®  
,
®®  !
Object
®®" (
>
®®( )
	PageState
®®* 3
{
®®4 5
get
®®6 9
;
®®9 :
private
®®; B
set
®®C F
;
®®F G
}
®®H I
public
ÆÆ  
SaveStateEventArgs
ÆÆ !
(
ÆÆ! "

Dictionary
ÆÆ" ,
<
ÆÆ, -
string
ÆÆ- 3
,
ÆÆ3 4
Object
ÆÆ5 ;
>
ÆÆ; <
	pageState
ÆÆ= F
)
ÆÆF G
:
ØØ 
base
ØØ 
(
ØØ 
)
ØØ 
{
∞∞ 	
this
±± 
.
±± 
	PageState
±± 
=
±± 
	pageState
±± &
;
±±& '
}
≤≤ 	
}
≥≥ 
}¥¥ ±
PC:\Sources\Other\ModernKeePass\ModernKeePass\Common\NotifyPropertyChangedBase.cs
	namespace 	
ModernKeePass
 
. 
Common 
{ 
public 

class %
NotifyPropertyChangedBase *
:+ ,"
INotifyPropertyChanged- C
{ 
public		 
event		 '
PropertyChangedEventHandler		 0
PropertyChanged		1 @
;		@ A
	protected 
void 
OnPropertyChanged (
(( )
string) /
propertyName0 <
== >
$str? A
)A B
{ 	
PropertyChanged 
? 
. 
Invoke #
(# $
this$ (
,( )
new* -$
PropertyChangedEventArgs. F
(F G
propertyNameG S
)S T
)T U
;U V
} 	
	protected 
bool 
SetProperty "
<" #
T# $
>$ %
(% &
ref& )
T* +
property, 4
,4 5
T6 7
value8 =
,= >
[? @
CallerMemberName@ P
]P Q
stringR X
propertyNameY e
=f g
$strh j
)j k
{ 	
if 
( 
EqualityComparer  
<  !
T! "
>" #
.# $
Default$ +
.+ ,
Equals, 2
(2 3
property3 ;
,; <
value= B
)B C
)C D
{ 
return 
false 
; 
} 
property 
= 
value 
; 
OnPropertyChanged 
( 
propertyName *
)* +
;+ ,
return 
true 
; 
} 	
} 
} π`
KC:\Sources\Other\ModernKeePass\ModernKeePass\Common\ObservableDictionary.cs
	namespace 	
ModernKeePass
 
. 
Common 
{ 
public 

class  
ObservableDictionary %
:& '
IObservableMap( 6
<6 7
string7 =
,= >
object? E
>E F
{ 
private 
class 0
$ObservableDictionaryChangedEventArgs :
:; < 
IMapChangedEventArgs= Q
<Q R
stringR X
>X Y
{ 	
public 0
$ObservableDictionaryChangedEventArgs 7
(7 8
CollectionChange8 H
changeI O
,O P
stringQ W
keyX [
)[ \
{ 
this 
. 
CollectionChange %
=& '
change( .
;. /
this 
. 
Key 
= 
key 
; 
} 
public 
CollectionChange #
CollectionChange$ 4
{5 6
get7 :
;: ;
private< C
setD G
;G H
}I J
public 
string 
Key 
{ 
get  #
;# $
private% ,
set- 0
;0 1
}2 3
} 	
private 

Dictionary 
< 
string !
,! "
object# )
>) *
_dictionary+ 6
=7 8
new9 <

Dictionary= G
<G H
stringH N
,N O
objectP V
>V W
(W X
)X Y
;Y Z
public 
event "
MapChangedEventHandler +
<+ ,
string, 2
,2 3
object4 :
>: ;

MapChanged< F
;F G
private 
void 
InvokeMapChanged %
(% &
CollectionChange& 6
change7 =
,= >
string? E
keyF I
)I J
{ 	
var 
eventHandler 
= 

MapChanged )
;) *
if   
(   
eventHandler   
!=   
null    $
)  $ %
{!! 
eventHandler"" 
("" 
this"" !
,""! "
new""# &0
$ObservableDictionaryChangedEventArgs""' K
(""K L
change""L R
,""R S
key""T W
)""W X
)""X Y
;""Y Z
}## 
}$$ 	
public&& 
void&& 
Add&& 
(&& 
string&& 
key&& "
,&&" #
object&&$ *
value&&+ 0
)&&0 1
{'' 	
this(( 
.(( 
_dictionary(( 
.(( 
Add((  
(((  !
key((! $
,(($ %
value((& +
)((+ ,
;((, -
this)) 
.)) 
InvokeMapChanged)) !
())! "
CollectionChange))" 2
.))2 3
ItemInserted))3 ?
,))? @
key))A D
)))D E
;))E F
}** 	
public,, 
void,, 
Add,, 
(,, 
KeyValuePair,, $
<,,$ %
string,,% +
,,,+ ,
object,,- 3
>,,3 4
item,,5 9
),,9 :
{-- 	
this.. 
... 
Add.. 
(.. 
item.. 
... 
Key.. 
,.. 
item.. #
...# $
Value..$ )
)..) *
;..* +
}// 	
public11 
void11 
AddRange11 
(11 
IEnumerable11 (
<11( )
KeyValuePair11) 5
<115 6
string116 <
,11< =
object11> D
>11D E
>11E F
values11G M
)11M N
{22 	
foreach33 
(33 
var33 
value33 
in33 !
values33" (
)33( )
{44 
Add55 
(55 
value55 
)55 
;55 
}66 
}77 	
public99 
bool99 
Remove99 
(99 
string99 !
key99" %
)99% &
{:: 	
if;; 
(;; 
this;; 
.;; 
_dictionary;;  
.;;  !
Remove;;! '
(;;' (
key;;( +
);;+ ,
);;, -
{<< 
this== 
.== 
InvokeMapChanged== %
(==% &
CollectionChange==& 6
.==6 7
ItemRemoved==7 B
,==B C
key==D G
)==G H
;==H I
return>> 
true>> 
;>> 
}?? 
return@@ 
false@@ 
;@@ 
}AA 	
publicCC 
boolCC 
RemoveCC 
(CC 
KeyValuePairCC '
<CC' (
stringCC( .
,CC. /
objectCC0 6
>CC6 7
itemCC8 <
)CC< =
{DD 	
objectEE 
currentValueEE 
;EE  
ifFF 
(FF 
thisFF 
.FF 
_dictionaryFF  
.FF  !
TryGetValueFF! ,
(FF, -
itemFF- 1
.FF1 2
KeyFF2 5
,FF5 6
outFF7 :
currentValueFF; G
)FFG H
&&FFI K
ObjectGG 
.GG 
EqualsGG 
(GG 
itemGG "
.GG" #
ValueGG# (
,GG( )
currentValueGG* 6
)GG6 7
&&GG8 :
thisGG; ?
.GG? @
_dictionaryGG@ K
.GGK L
RemoveGGL R
(GGR S
itemGGS W
.GGW X
KeyGGX [
)GG[ \
)GG\ ]
{HH 
thisII 
.II 
InvokeMapChangedII %
(II% &
CollectionChangeII& 6
.II6 7
ItemRemovedII7 B
,IIB C
itemIID H
.IIH I
KeyIII L
)IIL M
;IIM N
returnJJ 
trueJJ 
;JJ 
}KK 
returnLL 
falseLL 
;LL 
}MM 	
publicOO 
objectOO 
thisOO 
[OO 
stringOO !
keyOO" %
]OO% &
{PP 	
getQQ 
{RR 
returnSS 
thisSS 
.SS 
_dictionarySS '
[SS' (
keySS( +
]SS+ ,
;SS, -
}TT 
setUU 
{VV 
thisWW 
.WW 
_dictionaryWW  
[WW  !
keyWW! $
]WW$ %
=WW& '
valueWW( -
;WW- .
thisXX 
.XX 
InvokeMapChangedXX %
(XX% &
CollectionChangeXX& 6
.XX6 7
ItemChangedXX7 B
,XXB C
keyXXD G
)XXG H
;XXH I
}YY 
}ZZ 	
public\\ 
void\\ 
Clear\\ 
(\\ 
)\\ 
{]] 	
var^^ 
	priorKeys^^ 
=^^ 
this^^  
.^^  !
_dictionary^^! ,
.^^, -
Keys^^- 1
.^^1 2
ToArray^^2 9
(^^9 :
)^^: ;
;^^; <
this__ 
.__ 
_dictionary__ 
.__ 
Clear__ "
(__" #
)__# $
;__$ %
foreach`` 
(`` 
var`` 
key`` 
in`` 
	priorKeys``  )
)``) *
{aa 
thisbb 
.bb 
InvokeMapChangedbb %
(bb% &
CollectionChangebb& 6
.bb6 7
ItemRemovedbb7 B
,bbB C
keybbD G
)bbG H
;bbH I
}cc 
}dd 	
publicff 
ICollectionff 
<ff 
stringff !
>ff! "
Keysff# '
{gg 	
gethh 
{hh 
returnhh 
thishh 
.hh 
_dictionaryhh )
.hh) *
Keyshh* .
;hh. /
}hh0 1
}ii 	
publickk 
boolkk 
ContainsKeykk 
(kk  
stringkk  &
keykk' *
)kk* +
{ll 	
returnmm 
thismm 
.mm 
_dictionarymm #
.mm# $
ContainsKeymm$ /
(mm/ 0
keymm0 3
)mm3 4
;mm4 5
}nn 	
publicpp 
boolpp 
TryGetValuepp 
(pp  
stringpp  &
keypp' *
,pp* +
outpp, /
objectpp0 6
valuepp7 <
)pp< =
{qq 	
returnrr 
thisrr 
.rr 
_dictionaryrr #
.rr# $
TryGetValuerr$ /
(rr/ 0
keyrr0 3
,rr3 4
outrr5 8
valuerr9 >
)rr> ?
;rr? @
}ss 	
publicuu 
ICollectionuu 
<uu 
objectuu !
>uu! "
Valuesuu# )
{vv 	
getww 
{ww 
returnww 
thisww 
.ww 
_dictionaryww )
.ww) *
Valuesww* 0
;ww0 1
}ww2 3
}xx 	
publiczz 
boolzz 
Containszz 
(zz 
KeyValuePairzz )
<zz) *
stringzz* 0
,zz0 1
objectzz2 8
>zz8 9
itemzz: >
)zz> ?
{{{ 	
return|| 
this|| 
.|| 
_dictionary|| #
.||# $
Contains||$ ,
(||, -
item||- 1
)||1 2
;||2 3
}}} 	
public 
int 
Count 
{
ÄÄ 	
get
ÅÅ 
{
ÅÅ 
return
ÅÅ 
this
ÅÅ 
.
ÅÅ 
_dictionary
ÅÅ )
.
ÅÅ) *
Count
ÅÅ* /
;
ÅÅ/ 0
}
ÅÅ1 2
}
ÇÇ 	
public
ÑÑ 
bool
ÑÑ 

IsReadOnly
ÑÑ 
{
ÖÖ 	
get
ÜÜ 
{
ÜÜ 
return
ÜÜ 
false
ÜÜ 
;
ÜÜ 
}
ÜÜ  !
}
áá 	
public
ââ 
IEnumerator
ââ 
<
ââ 
KeyValuePair
ââ '
<
ââ' (
string
ââ( .
,
ââ. /
object
ââ0 6
>
ââ6 7
>
ââ7 8
GetEnumerator
ââ9 F
(
ââF G
)
ââG H
{
ää 	
return
ãã 
this
ãã 
.
ãã 
_dictionary
ãã #
.
ãã# $
GetEnumerator
ãã$ 1
(
ãã1 2
)
ãã2 3
;
ãã3 4
}
åå 	
System
éé 
.
éé 
Collections
éé 
.
éé 
IEnumerator
éé &
System
éé' -
.
éé- .
Collections
éé. 9
.
éé9 :
IEnumerable
éé: E
.
ééE F
GetEnumerator
ééF S
(
ééS T
)
ééT U
{
èè 	
return
êê 
this
êê 
.
êê 
_dictionary
êê #
.
êê# $
GetEnumerator
êê$ 1
(
êê1 2
)
êê2 3
;
êê3 4
}
ëë 	
public
ìì 
void
ìì 
CopyTo
ìì 
(
ìì 
KeyValuePair
ìì '
<
ìì' (
string
ìì( .
,
ìì. /
object
ìì0 6
>
ìì6 7
[
ìì7 8
]
ìì8 9
array
ìì: ?
,
ìì? @
int
ììA D

arrayIndex
ììE O
)
ììO P
{
îî 	
int
ïï 
	arraySize
ïï 
=
ïï 
array
ïï !
.
ïï! "
Length
ïï" (
;
ïï( )
foreach
ññ 
(
ññ 
var
ññ 
pair
ññ 
in
ññ  
this
ññ! %
.
ññ% &
_dictionary
ññ& 1
)
ññ1 2
{
óó 
if
òò 
(
òò 

arrayIndex
òò 
>=
òò !
	arraySize
òò" +
)
òò+ ,
break
òò- 2
;
òò2 3
array
ôô 
[
ôô 

arrayIndex
ôô  
++
ôô  "
]
ôô" #
=
ôô$ %
pair
ôô& *
;
ôô* +
}
öö 
}
õõ 	
}
úú 
}ùù ¸
CC:\Sources\Other\ModernKeePass\ModernKeePass\Common\RelayCommand.cs
	namespace 	
ModernKeePass
 
. 
Common 
{		 
public 

class 
RelayCommand 
: 
ICommand  (
{ 
private 
readonly 
Action 
_execute  (
;( )
private 
readonly 
Func 
< 
bool "
>" #
_canExecute$ /
;/ 0
public 
event 
EventHandler !
CanExecuteChanged" 3
;3 4
public 
RelayCommand 
( 
Action "
execute# *
)* +
:   
this   
(   
execute   
,   
null    
)    !
{!! 	
}"" 	
public)) 
RelayCommand)) 
()) 
Action)) "
execute))# *
,))* +
Func)), 0
<))0 1
bool))1 5
>))5 6

canExecute))7 A
)))A B
{** 	
if++ 
(++ 
execute++ 
==++ 
null++ 
)++  
throw,, 
new,, !
ArgumentNullException,, /
(,,/ 0
$str,,0 9
),,9 :
;,,: ;
_execute-- 
=-- 
execute-- 
;-- 
_canExecute.. 
=.. 

canExecute.. $
;..$ %
}// 	
public88 
bool88 

CanExecute88 
(88 
object88 %
	parameter88& /
)88/ 0
{99 	
return:: 
_canExecute:: 
==:: !
null::" &
?::' (
true::) -
:::. /
_canExecute::0 ;
(::; <
)::< =
;::= >
};; 	
publicCC 
voidCC 
ExecuteCC 
(CC 
objectCC "
	parameterCC# ,
)CC, -
{DD 	
_executeEE 
(EE 
)EE 
;EE 
}FF 	
publicMM 
voidMM "
RaiseCanExecuteChangedMM *
(MM* +
)MM+ ,
{NN 	
varOO 
handlerOO 
=OO 
CanExecuteChangedOO +
;OO+ ,
ifPP 
(PP 
handlerPP 
!=PP 
nullPP 
)PP  
{QQ 
handlerRR 
(RR 
thisRR 
,RR 
	EventArgsRR '
.RR' (
EmptyRR( -
)RR- .
;RR. /
}SS 
}TT 	
}UU 
}VV ¬ã
HC:\Sources\Other\ModernKeePass\ModernKeePass\Common\SuspensionManager.cs
	namespace 	
ModernKeePass
 
. 
Common 
{ 
internal 
sealed 
class 
SuspensionManager +
{ 
private 
static 

Dictionary !
<! "
string" (
,( )
object* 0
>0 1
_sessionState2 ?
=@ A
newB E

DictionaryF P
<P Q
stringQ W
,W X
objectY _
>_ `
(` a
)a b
;b c
private 
static 
List 
< 
Type  
>  !
_knownTypes" -
=. /
new0 3
List4 8
<8 9
Type9 =
>= >
(> ?
)? @
;@ A
private 
const 
string  
sessionStateFilename 1
=2 3
$str4 G
;G H
public$$ 
static$$ 

Dictionary$$  
<$$  !
string$$! '
,$$' (
object$$) /
>$$/ 0
SessionState$$1 =
{%% 	
get&& 
{&& 
return&& 
_sessionState&& &
;&&& '
}&&( )
}'' 	
public.. 
static.. 
List.. 
<.. 
Type.. 
>..  

KnownTypes..! +
{// 	
get00 
{00 
return00 
_knownTypes00 $
;00$ %
}00& '
}11 	
public:: 
static:: 
async:: 
Task::  
	SaveAsync::! *
(::* +
)::+ ,
{;; 	
try<< 
{== 
foreach?? 
(?? 
var?? 
weakFrameReference?? /
in??0 2
_registeredFrames??3 D
)??D E
{@@ 
FrameAA 
frameAA 
;AA  
ifBB 
(BB 
weakFrameReferenceBB *
.BB* +
TryGetTargetBB+ 7
(BB7 8
outBB8 ;
frameBB< A
)BBA B
)BBB C
{CC $
SaveFrameNavigationStateDD 0
(DD0 1
frameDD1 6
)DD6 7
;DD7 8
}EE 
}FF 
MemoryStreamJJ 
sessionDataJJ (
=JJ) *
newJJ+ .
MemoryStreamJJ/ ;
(JJ; <
)JJ< =
;JJ= >"
DataContractSerializerKK &

serializerKK' 1
=KK2 3
newKK4 7"
DataContractSerializerKK8 N
(KKN O
typeofKKO U
(KKU V

DictionaryKKV `
<KK` a
stringKKa g
,KKg h
objectKKi o
>KKo p
)KKp q
,KKq r
_knownTypesKKs ~
)KK~ 
;	KK Ä

serializerLL 
.LL 
WriteObjectLL &
(LL& '
sessionDataLL' 2
,LL2 3
_sessionStateLL4 A
)LLA B
;LLB C
StorageFileOO 
fileOO  
=OO! "
awaitOO# (
ApplicationDataOO) 8
.OO8 9
CurrentOO9 @
.OO@ A
LocalFolderOOA L
.OOL M
CreateFileAsyncOOM \
(OO\ ] 
sessionStateFilenameOO] q
,OOq r$
CreationCollisionOption	OOs ä
.
OOä ã
ReplaceExisting
OOã ö
)
OOö õ
;
OOõ ú
usingPP 
(PP 
StreamPP 

fileStreamPP (
=PP) *
awaitPP+ 0
filePP1 5
.PP5 6#
OpenStreamForWriteAsyncPP6 M
(PPM N
)PPN O
)PPO P
{QQ 
sessionDataRR 
.RR  
SeekRR  $
(RR$ %
$numRR% &
,RR& '

SeekOriginRR( 2
.RR2 3
BeginRR3 8
)RR8 9
;RR9 :
awaitSS 
sessionDataSS %
.SS% &
CopyToAsyncSS& 1
(SS1 2

fileStreamSS2 <
)SS< =
;SS= >
}TT 
}UU 
catchVV 
(VV 
	ExceptionVV 
eVV 
)VV 
{WW 
throwXX 
newXX &
SuspensionManagerExceptionXX 4
(XX4 5
eXX5 6
)XX6 7
;XX7 8
}YY 
}ZZ 	
publicgg 
staticgg 
asyncgg 
Taskgg  
RestoreAsyncgg! -
(gg- .
Stringgg. 4
sessionBaseKeygg5 C
=ggD E
nullggF J
)ggJ K
{hh 	
_sessionStateii 
=ii 
newii 

Dictionaryii  *
<ii* +
Stringii+ 1
,ii1 2
Objectii3 9
>ii9 :
(ii: ;
)ii; <
;ii< =
trykk 
{ll 
StorageFilenn 
filenn  
=nn! "
awaitnn# (
ApplicationDatann) 8
.nn8 9
Currentnn9 @
.nn@ A
LocalFoldernnA L
.nnL M
GetFileAsyncnnM Y
(nnY Z 
sessionStateFilenamennZ n
)nnn o
;nno p
usingoo 
(oo 
IInputStreamoo #
inStreamoo$ ,
=oo- .
awaitoo/ 4
fileoo5 9
.oo9 :#
OpenSequentialReadAsyncoo: Q
(ooQ R
)ooR S
)ooS T
{pp "
DataContractSerializerrr *

serializerrr+ 5
=rr6 7
newrr8 ;"
DataContractSerializerrr< R
(rrR S
typeofrrS Y
(rrY Z

DictionaryrrZ d
<rrd e
stringrre k
,rrk l
objectrrm s
>rrs t
)rrt u
,rru v
_knownTypes	rrw Ç
)
rrÇ É
;
rrÉ Ñ
_sessionStatess !
=ss" #
(ss$ %

Dictionaryss% /
<ss/ 0
stringss0 6
,ss6 7
objectss8 >
>ss> ?
)ss? @

serializerss@ J
.ssJ K

ReadObjectssK U
(ssU V
inStreamssV ^
.ss^ _
AsStreamForReadss_ n
(ssn o
)sso p
)ssp q
;ssq r
}tt 
foreachww 
(ww 
varww 
weakFrameReferenceww /
inww0 2
_registeredFramesww3 D
)wwD E
{xx 
Frameyy 
frameyy 
;yy  
ifzz 
(zz 
weakFrameReferencezz *
.zz* +
TryGetTargetzz+ 7
(zz7 8
outzz8 ;
framezz< A
)zzA B
&&zzC E
(zzF G
stringzzG M
)zzM N
framezzN S
.zzS T
GetValuezzT \
(zz\ ]'
FrameSessionBaseKeyPropertyzz] x
)zzx y
==zzz |
sessionBaseKey	zz} ã
)
zzã å
{{{ 
frame|| 
.|| 

ClearValue|| (
(||( )%
FrameSessionStateProperty||) B
)||B C
;||C D'
RestoreFrameNavigationState}} 3
(}}3 4
frame}}4 9
)}}9 :
;}}: ;
}~~ 
} 
}
ÄÄ 
catch
ÅÅ 
(
ÅÅ 
	Exception
ÅÅ 
e
ÅÅ 
)
ÅÅ 
{
ÇÇ 
throw
ÉÉ 
new
ÉÉ (
SuspensionManagerException
ÉÉ 4
(
ÉÉ4 5
e
ÉÉ5 6
)
ÉÉ6 7
;
ÉÉ7 8
}
ÑÑ 
}
ÖÖ 	
private
áá 
static
áá  
DependencyProperty
áá )*
FrameSessionStateKeyProperty
áá* F
=
ááG H 
DependencyProperty
àà 
.
àà 
RegisterAttached
àà /
(
àà/ 0
$str
àà0 G
,
ààG H
typeof
ààI O
(
ààO P
String
ààP V
)
ààV W
,
ààW X
typeof
ààY _
(
àà_ `
SuspensionManager
àà` q
)
ààq r
,
ààr s
null
ààt x
)
ààx y
;
àày z
private
ââ 
static
ââ  
DependencyProperty
ââ ))
FrameSessionBaseKeyProperty
ââ* E
=
ââF G 
DependencyProperty
ää 
.
ää 
RegisterAttached
ää /
(
ää/ 0
$str
ää0 L
,
ääL M
typeof
ääN T
(
ääT U
String
ääU [
)
ää[ \
,
ää\ ]
typeof
ää^ d
(
ääd e
SuspensionManager
ääe v
)
ääv w
,
ääw x
null
ääy }
)
ää} ~
;
ää~ 
private
ãã 
static
ãã  
DependencyProperty
ãã )'
FrameSessionStateProperty
ãã* C
=
ããD E 
DependencyProperty
åå 
.
åå 
RegisterAttached
åå /
(
åå/ 0
$str
åå0 D
,
ååD E
typeof
ååF L
(
ååL M

Dictionary
ååM W
<
ååW X
String
ååX ^
,
åå^ _
Object
åå` f
>
ååf g
)
ååg h
,
ååh i
typeof
ååj p
(
ååp q 
SuspensionManagerååq Ç
)ååÇ É
,ååÉ Ñ
nullååÖ â
)ååâ ä
;ååä ã
private
çç 
static
çç 
List
çç 
<
çç 
WeakReference
çç )
<
çç) *
Frame
çç* /
>
çç/ 0
>
çç0 1
_registeredFrames
çç2 C
=
ççD E
new
ççF I
List
ççJ N
<
ççN O
WeakReference
ççO \
<
çç\ ]
Frame
çç] b
>
ççb c
>
ççc d
(
ççd e
)
ççe f
;
ççf g
public
ùù 
static
ùù 
void
ùù 
RegisterFrame
ùù (
(
ùù( )
Frame
ùù) .
frame
ùù/ 4
,
ùù4 5
String
ùù6 <
sessionStateKey
ùù= L
,
ùùL M
String
ùùN T
sessionBaseKey
ùùU c
=
ùùd e
null
ùùf j
)
ùùj k
{
ûû 	
if
üü 
(
üü 
frame
üü 
.
üü 
GetValue
üü 
(
üü *
FrameSessionStateKeyProperty
üü ;
)
üü; <
!=
üü= ?
null
üü@ D
)
üüD E
{
†† 
throw
°° 
new
°° '
InvalidOperationException
°° 3
(
°°3 4
$str
°°4 l
)
°°l m
;
°°m n
}
¢¢ 
if
§§ 
(
§§ 
frame
§§ 
.
§§ 
GetValue
§§ 
(
§§ '
FrameSessionStateProperty
§§ 8
)
§§8 9
!=
§§: <
null
§§= A
)
§§A B
{
•• 
throw
¶¶ 
new
¶¶ '
InvalidOperationException
¶¶ 3
(
¶¶3 4
$str¶¶4 ò
)¶¶ò ô
;¶¶ô ö
}
ßß 
if
©© 
(
©© 
!
©© 
string
©© 
.
©© 
IsNullOrEmpty
©© %
(
©©% &
sessionBaseKey
©©& 4
)
©©4 5
)
©©5 6
{
™™ 
frame
´´ 
.
´´ 
SetValue
´´ 
(
´´ )
FrameSessionBaseKeyProperty
´´ :
,
´´: ;
sessionBaseKey
´´< J
)
´´J K
;
´´K L
sessionStateKey
¨¨ 
=
¨¨  !
sessionBaseKey
¨¨" 0
+
¨¨1 2
$str
¨¨3 6
+
¨¨7 8
sessionStateKey
¨¨9 H
;
¨¨H I
}
≠≠ 
frame
±± 
.
±± 
SetValue
±± 
(
±± *
FrameSessionStateKeyProperty
±± 7
,
±±7 8
sessionStateKey
±±9 H
)
±±H I
;
±±I J
_registeredFrames
≤≤ 
.
≤≤ 
Add
≤≤ !
(
≤≤! "
new
≤≤" %
WeakReference
≤≤& 3
<
≤≤3 4
Frame
≤≤4 9
>
≤≤9 :
(
≤≤: ;
frame
≤≤; @
)
≤≤@ A
)
≤≤A B
;
≤≤B C)
RestoreFrameNavigationState
µµ '
(
µµ' (
frame
µµ( -
)
µµ- .
;
µµ. /
}
∂∂ 	
public
øø 
static
øø 
void
øø 
UnregisterFrame
øø *
(
øø* +
Frame
øø+ 0
frame
øø1 6
)
øø6 7
{
¿¿ 	
SessionState
√√ 
.
√√ 
Remove
√√ 
(
√√  
(
√√  !
String
√√! '
)
√√' (
frame
√√( -
.
√√- .
GetValue
√√. 6
(
√√6 7*
FrameSessionStateKeyProperty
√√7 S
)
√√S T
)
√√T U
;
√√U V
_registeredFrames
ƒƒ 
.
ƒƒ 
	RemoveAll
ƒƒ '
(
ƒƒ' (
(
ƒƒ( ) 
weakFrameReference
ƒƒ) ;
)
ƒƒ; <
=>
ƒƒ= ?
{
≈≈ 
Frame
∆∆ 
	testFrame
∆∆ 
;
∆∆  
return
«« 
!
««  
weakFrameReference
«« *
.
««* +
TryGetTarget
««+ 7
(
««7 8
out
««8 ;
	testFrame
««< E
)
««E F
||
««G I
	testFrame
««J S
==
««T V
frame
««W \
;
««\ ]
}
»» 
)
»» 
;
»» 
}
…… 	
public
ÿÿ 
static
ÿÿ 

Dictionary
ÿÿ  
<
ÿÿ  !
String
ÿÿ! '
,
ÿÿ' (
Object
ÿÿ) /
>
ÿÿ/ 0"
SessionStateForFrame
ÿÿ1 E
(
ÿÿE F
Frame
ÿÿF K
frame
ÿÿL Q
)
ÿÿQ R
{
ŸŸ 	
var
⁄⁄ 

frameState
⁄⁄ 
=
⁄⁄ 
(
⁄⁄ 

Dictionary
⁄⁄ (
<
⁄⁄( )
String
⁄⁄) /
,
⁄⁄/ 0
Object
⁄⁄1 7
>
⁄⁄7 8
)
⁄⁄8 9
frame
⁄⁄9 >
.
⁄⁄> ?
GetValue
⁄⁄? G
(
⁄⁄G H'
FrameSessionStateProperty
⁄⁄H a
)
⁄⁄a b
;
⁄⁄b c
if
‹‹ 
(
‹‹ 

frameState
‹‹ 
==
‹‹ 
null
‹‹ "
)
‹‹" #
{
›› 
var
ﬁﬁ 
frameSessionKey
ﬁﬁ #
=
ﬁﬁ$ %
(
ﬁﬁ& '
String
ﬁﬁ' -
)
ﬁﬁ- .
frame
ﬁﬁ. 3
.
ﬁﬁ3 4
GetValue
ﬁﬁ4 <
(
ﬁﬁ< =*
FrameSessionStateKeyProperty
ﬁﬁ= Y
)
ﬁﬁY Z
;
ﬁﬁZ [
if
ﬂﬂ 
(
ﬂﬂ 
frameSessionKey
ﬂﬂ #
!=
ﬂﬂ$ &
null
ﬂﬂ' +
)
ﬂﬂ+ ,
{
‡‡ 
if
‚‚ 
(
‚‚ 
!
‚‚ 
_sessionState
‚‚ &
.
‚‚& '
ContainsKey
‚‚' 2
(
‚‚2 3
frameSessionKey
‚‚3 B
)
‚‚B C
)
‚‚C D
{
„„ 
_sessionState
‰‰ %
[
‰‰% &
frameSessionKey
‰‰& 5
]
‰‰5 6
=
‰‰7 8
new
‰‰9 <

Dictionary
‰‰= G
<
‰‰G H
String
‰‰H N
,
‰‰N O
Object
‰‰P V
>
‰‰V W
(
‰‰W X
)
‰‰X Y
;
‰‰Y Z
}
ÂÂ 

frameState
ÊÊ 
=
ÊÊ  
(
ÊÊ! "

Dictionary
ÊÊ" ,
<
ÊÊ, -
String
ÊÊ- 3
,
ÊÊ3 4
Object
ÊÊ5 ;
>
ÊÊ; <
)
ÊÊ< =
_sessionState
ÊÊ= J
[
ÊÊJ K
frameSessionKey
ÊÊK Z
]
ÊÊZ [
;
ÊÊ[ \
}
ÁÁ 
else
ËË 
{
ÈÈ 

frameState
ÎÎ 
=
ÎÎ  
new
ÎÎ! $

Dictionary
ÎÎ% /
<
ÎÎ/ 0
String
ÎÎ0 6
,
ÎÎ6 7
Object
ÎÎ8 >
>
ÎÎ> ?
(
ÎÎ? @
)
ÎÎ@ A
;
ÎÎA B
}
ÏÏ 
frame
ÌÌ 
.
ÌÌ 
SetValue
ÌÌ 
(
ÌÌ '
FrameSessionStateProperty
ÌÌ 8
,
ÌÌ8 9

frameState
ÌÌ: D
)
ÌÌD E
;
ÌÌE F
}
ÓÓ 
return
ÔÔ 

frameState
ÔÔ 
;
ÔÔ 
}
 	
private
ÚÚ 
static
ÚÚ 
void
ÚÚ )
RestoreFrameNavigationState
ÚÚ 7
(
ÚÚ7 8
Frame
ÚÚ8 =
frame
ÚÚ> C
)
ÚÚC D
{
ÛÛ 	
var
ÙÙ 

frameState
ÙÙ 
=
ÙÙ "
SessionStateForFrame
ÙÙ 1
(
ÙÙ1 2
frame
ÙÙ2 7
)
ÙÙ7 8
;
ÙÙ8 9
if
ıı 
(
ıı 

frameState
ıı 
.
ıı 
ContainsKey
ıı &
(
ıı& '
$str
ıı' 3
)
ıı3 4
)
ıı4 5
{
ˆˆ 
frame
˜˜ 
.
˜˜  
SetNavigationState
˜˜ (
(
˜˜( )
(
˜˜) *
String
˜˜* 0
)
˜˜0 1

frameState
˜˜1 ;
[
˜˜; <
$str
˜˜< H
]
˜˜H I
)
˜˜I J
;
˜˜J K
}
¯¯ 
}
˘˘ 	
private
˚˚ 
static
˚˚ 
void
˚˚ &
SaveFrameNavigationState
˚˚ 4
(
˚˚4 5
Frame
˚˚5 :
frame
˚˚; @
)
˚˚@ A
{
¸¸ 	
var
˝˝ 

frameState
˝˝ 
=
˝˝ "
SessionStateForFrame
˝˝ 1
(
˝˝1 2
frame
˝˝2 7
)
˝˝7 8
;
˝˝8 9

frameState
˛˛ 
[
˛˛ 
$str
˛˛ #
]
˛˛# $
=
˛˛% &
frame
˛˛' ,
.
˛˛, - 
GetNavigationState
˛˛- ?
(
˛˛? @
)
˛˛@ A
;
˛˛A B
}
ˇˇ 	
}
ÄÄ 
public
ÅÅ 

class
ÅÅ (
SuspensionManagerException
ÅÅ +
:
ÅÅ, -
	Exception
ÅÅ. 7
{
ÇÇ 
public
ÉÉ (
SuspensionManagerException
ÉÉ )
(
ÉÉ) *
)
ÉÉ* +
{
ÑÑ 	
}
ÖÖ 	
public
áá (
SuspensionManagerException
áá )
(
áá) *
	Exception
áá* 3
e
áá4 5
)
áá5 6
:
àà 
base
àà 
(
àà 
$str
àà -
,
àà- .
e
àà/ 0
)
àà0 1
{
ââ 	
}
ãã 	
}
åå 
}çç ¢-
GC:\Sources\Other\ModernKeePass\ModernKeePass\Services\LicenseService.cs
	namespace 	
ModernKeePass
 
. 
Services  
{ 
public		 

class		 
LicenseService		 
:		  ! 
SingletonServiceBase		" 6
<		6 7
LicenseService		7 E
>		E F
,		F G
ILicenseService		H W
{

 
public 
enum 
PurchaseResult "
{ 	
	Succeeded 
, 
NothingToFulfill 
, 
PurchasePending 
, 
PurchaseReverted 
, 
ServerError 
, 
NotPurchased 
, 
AlreadyPurchased 
} 	
public 
IReadOnlyDictionary "
<" #
string# )
,) *
ProductListing+ 9
>9 :
Products; C
{D E
getF I
;I J
}K L
private 
readonly 
HashSet  
<  !
Guid! %
>% &#
_consumedTransactionIds' >
=? @
newA D
HashSetE L
<L M
GuidM Q
>Q R
(R S
)S T
;T U
public 
LicenseService 
( 
) 
{ 	
var 
listing 
= 

CurrentApp $
.$ %'
LoadListingInformationAsync% @
(@ A
)A B
.B C

GetAwaiterC M
(M N
)N O
.O P
	GetResultP Y
(Y Z
)Z [
;[ \
Products 
= 
listing 
. 
ProductListings .
;. /
} 	
public!! 
async!! 
Task!! 
<!! 
int!! 
>!! 
Purchase!! '
(!!' (
string!!( .
addOn!!/ 4
)!!4 5
{"" 	
var## 
purchaseResults## 
=##  !
await##" '

CurrentApp##( 2
.##2 3'
RequestProductPurchaseAsync##3 N
(##N O
addOn##O T
)##T U
;##U V
switch$$ 
($$ 
purchaseResults$$ #
.$$# $
Status$$$ *
)$$* +
{%% 
case&& !
ProductPurchaseStatus&& *
.&&* +
	Succeeded&&+ 4
:&&4 5
GrantFeatureLocally'' '
(''' (
purchaseResults''( 7
.''7 8
TransactionId''8 E
)''E F
;''F G
return(( 
((( 
int(( 
)((  
await((! &"
ReportFulfillmentAsync((' =
(((= >
purchaseResults((> M
.((M N
TransactionId((N [
,(([ \
addOn((] b
)((b c
;((c d
case)) !
ProductPurchaseStatus)) *
.))* +
NotFulfilled))+ 7
:))7 8
if,, 
(,, 
!,, 
IsLocallyFulfilled,, +
(,,+ ,
purchaseResults,,, ;
.,,; <
TransactionId,,< I
),,I J
),,J K
{-- 
GrantFeatureLocally.. +
(..+ ,
purchaseResults.., ;
...; <
TransactionId..< I
)..I J
;..J K
}// 
return00 
(00 
int00 
)00  
await00! &"
ReportFulfillmentAsync00' =
(00= >
purchaseResults00> M
.00M N
TransactionId00N [
,00[ \
addOn00] b
)00b c
;00c d
case11 !
ProductPurchaseStatus11 *
.11* +
NotPurchased11+ 7
:117 8
return22 
(22 
int22 
)22  
PurchaseResult22! /
.22/ 0
NotPurchased220 <
;22< =
case33 !
ProductPurchaseStatus33 *
.33* +
AlreadyPurchased33+ ;
:33; <
return44 
(44 
int44 
)44  
PurchaseResult44! /
.44/ 0
AlreadyPurchased440 @
;44@ A
default55 
:55 
throw66 
new66 '
ArgumentOutOfRangeException66 9
(669 :
)66: ;
;66; <
}77 
}88 	
private:: 
async:: 
Task:: 
<:: 
PurchaseResult:: )
>::) *"
ReportFulfillmentAsync::+ A
(::A B
Guid::B F
transactionId::G T
,::T U
string::V \
productName::] h
)::h i
{;; 	
var<< 
result<< 
=<< 
await<< 

CurrentApp<< )
.<<) *,
 ReportConsumableFulfillmentAsync<<* J
(<<J K
productName<<K V
,<<V W
transactionId<<X e
)<<e f
;<<f g
return== 
(== 
PurchaseResult== "
)==" #
result==$ *
;==* +
}>> 	
private@@ 
void@@ 
GrantFeatureLocally@@ (
(@@( )
Guid@@) -
transactionId@@. ;
)@@; <
{AA 	#
_consumedTransactionIdsBB #
.BB# $
AddBB$ '
(BB' (
transactionIdBB( 5
)BB5 6
;BB6 7
}CC 	
privateEE 
boolEE 
IsLocallyFulfilledEE '
(EE' (
GuidEE( ,
transactionIdEE- :
)EE: ;
{FF 	
returnGG #
_consumedTransactionIdsGG *
.GG* +
ContainsGG+ 3
(GG3 4
transactionIdGG4 A
)GGA B
;GGB C
}HH 	
}II 
}JJ Û
FC:\Sources\Other\ModernKeePass\ModernKeePass\Services\RecentService.cs
	namespace		 	
ModernKeePass		
 
.		 
Services		  
{

 
public 

class 
RecentService 
:   
SingletonServiceBase! 5
<5 6
RecentService6 C
>C D
,D E
IRecentServiceF T
{ 
private 
readonly +
StorageItemMostRecentlyUsedList 8
_mru9 =
=> ?)
StorageApplicationPermissions@ ]
.] ^ 
MostRecentlyUsedList^ r
;r s
public 
int 

EntryCount 
=>  
_mru! %
.% &
Entries& -
.- .
Count. 3
;3 4
public  
ObservableCollection #
<# $
IRecentItem$ /
>/ 0
GetAllFiles1 <
(< =
bool= A
removeIfNonExistantB U
=V W
trueX \
)\ ]
{ 	
var 
result 
= 
new  
ObservableCollection 1
<1 2
IRecentItem2 =
>= >
(> ?
)? @
;@ A
foreach 
( 
var 
entry 
in !
_mru" &
.& '
Entries' .
). /
{ 
try 
{ 
var 
file 
= 
_mru #
.# $
GetFileAsync$ 0
(0 1
entry1 6
.6 7
Token7 <
,< =
AccessCacheOptions> P
.P Q$
SuppressAccessTimeUpdateQ i
)i j
.j k

GetAwaiterk u
(u v
)v w
.w x
	GetResult	x Å
(
Å Ç
)
Ç É
;
É Ñ
result 
. 
Add 
( 
new "
RecentItemVm# /
(/ 0
entry0 5
.5 6
Token6 ;
,; <
entry= B
.B C
MetadataC K
,K L
fileM Q
)Q R
)R S
;S T
} 
catch 
( 
	Exception  
)  !
{ 
if 
( 
removeIfNonExistant +
)+ ,
_mru- 1
.1 2
Remove2 8
(8 9
entry9 >
.> ?
Token? D
)D E
;E F
} 
} 
return   
result   
;   
}!! 	
public## 
void## 
Add## 
(## 
IStorageItem## $
file##% )
,##) *
string##+ 1
metadata##2 :
)##: ;
{$$ 	
_mru%% 
.%% 
Add%% 
(%% 
file%% 
,%% 
metadata%% #
)%%# $
;%%$ %
}&& 	
public(( 
void(( 
ClearAll(( 
((( 
)(( 
{)) 	
_mru** 
.** 
Clear** 
(** 
)** 
;** 
}++ 	
public-- 
async-- 
Task-- 
<-- 
IStorageItem-- &
>--& '
GetFileAsync--( 4
(--4 5
string--5 ;
token--< A
)--A B
{.. 	
return// 
await// 
_mru// 
.// 
GetFileAsync// *
(//* +
token//+ 0
)//0 1
;//1 2
}00 	
}11 
}22 Ï	
IC:\Sources\Other\ModernKeePass\ModernKeePass\Services\ResourcesService.cs
	namespace 	
ModernKeePass
 
. 
Services  
{ 
public 

class 
ResourcesService !
:! "
IResourceService# 3
{ 
private 
const 
string 
ResourceFileName -
=. /
$str0 <
;< =
private		 
readonly		 
ResourceLoader		 '
_resourceLoader		( 7
=		8 9
ResourceLoader		: H
.		H I
GetForCurrentView		I Z
(		Z [
)		[ \
;		\ ]
public 
string 
GetResourceValue &
(& '
string' -
key. 1
)1 2
{ 	
var 
resource 
= 
_resourceLoader *
.* +
	GetString+ 4
(4 5
$"5 7
/7 8
{8 9
ResourceFileName9 I
}I J
/J K
{K L
keyL O
}O P
"P Q
)Q R
;R S
return 
resource 
; 
} 	
} 
} ¯
HC:\Sources\Other\ModernKeePass\ModernKeePass\Services\SettingsService.cs
	namespace 	
ModernKeePass
 
. 
Services  
{ 
public 

class 
SettingsService  
:! " 
SingletonServiceBase# 7
<7 8
SettingsService8 G
>G H
,H I
ISettingsServiceJ Z
{		 
private

 
readonly

 
IPropertySet

 %
_values

& -
=

. /
ApplicationData

0 ?
.

? @
Current

@ G
.

G H
LocalSettings

H U
.

U V
Values

V \
;

\ ]
public 
T 

GetSetting 
< 
T 
> 
( 
string %
property& .
,. /
T0 1
defaultValue2 >
=? @
defaultA H
(H I
TI J
)J K
)K L
{ 	
try 
{ 
return 
( 
T 
) 
Convert !
.! "

ChangeType" ,
(, -
_values- 4
[4 5
property5 =
]= >
,> ?
typeof@ F
(F G
TG H
)H I
)I J
;J K
} 
catch 
(  
InvalidCastException '
)' (
{ 
return 
defaultValue #
;# $
} 
} 	
public 
void 

PutSetting 
< 
T  
>  !
(! "
string" (
property) 1
,1 2
T3 4
value5 :
): ;
{ 	
if 
( 
_values 
. 
ContainsKey #
(# $
property$ ,
), -
)- .
_values 
[ 
property  
]  !
=" #
value$ )
;) *
else 
_values 
. 
Add 
( 
property %
,% &
value' ,
), -
;- .
} 	
} 
} À.
NC:\Sources\Other\ModernKeePass\ModernKeePass\Common\ToastNotificationHelper.cs
	namespace 	
ModernKeePass
 
. 
Common 
{		 
public

 

static

 
class

 #
ToastNotificationHelper

 /
{ 
public 
static 
void 
ShowMovedToast )
() *
	IPwEntity* 3
entity4 :
,: ;
string< B
actionC I
,I J
stringK Q
textR V
)V W
{ 	
var 

entityType 
= 
entity #
is$ &
GroupVm' .
?/ 0
$str1 8
:9 :
$str; B
;B C
var 
notificationXml 
=  !$
ToastNotificationManager" :
.: ;
GetTemplateContent; M
(M N
ToastTemplateTypeN _
._ `
ToastText02` k
)k l
;l m
var 
toastElements 
= 
notificationXml  /
./ 0 
GetElementsByTagName0 D
(D E
$strE K
)K L
;L M
toastElements 
[ 
$num 
] 
. 
AppendChild (
(( )
notificationXml) 8
.8 9
CreateTextNode9 G
(G H
$"H J
{J K
actionK Q
}Q R
{S T

entityTypeT ^
}^ _
{` a
entitya g
.g h
Nameh l
}l m
"m n
)n o
)o p
;p q
toastElements 
[ 
$num 
] 
. 
AppendChild (
(( )
notificationXml) 8
.8 9
CreateTextNode9 G
(G H
textH L
)L M
)M N
;N O
var 
	toastNode 
= 
notificationXml +
.+ ,
SelectSingleNode, <
(< =
$str= E
)E F
;F G
var 
launch 
= 
new 

JsonObject '
{ 
{ 
$str 
, 
	JsonValue (
.( )
CreateStringValue) :
(: ;

entityType; E
)E F
}F G
,G H
{ 
$str 
, 
	JsonValue &
.& '
CreateStringValue' 8
(8 9
entity9 ?
.? @
Id@ B
)B C
}C D
} 
; 
( 
( 

XmlElement 
) 
	toastNode "
)" #
?# $
.$ %
SetAttribute% 1
(1 2
$str2 :
,: ;
launch< B
.B C
	StringifyC L
(L M
)M N
)N O
;O P
var 
toast 
= 
new 
ToastNotification -
(- .
notificationXml. =
)= >
{ 
ExpirationTime 
=  
DateTime! )
.) *
Now* -
.- .

AddSeconds. 8
(8 9
$num9 :
): ;
} 
; $
ToastNotificationManager   $
.  $ %
CreateToastNotifier  % 8
(  8 9
)  9 :
.  : ;
Show  ; ?
(  ? @
toast  @ E
)  E F
;  F G
}!! 	
public## 
static## 
void## 
ShowGenericToast## +
(##+ ,
string##, 2
title##3 8
,##8 9
string##: @
text##A E
)##E F
{$$ 	
var%% 
notificationXml%% 
=%%  !$
ToastNotificationManager%%" :
.%%: ;
GetTemplateContent%%; M
(%%M N
ToastTemplateType%%N _
.%%_ `
ToastText02%%` k
)%%k l
;%%l m
var&& 
toastElements&& 
=&& 
notificationXml&&  /
.&&/ 0 
GetElementsByTagName&&0 D
(&&D E
$str&&E K
)&&K L
;&&L M
toastElements'' 
['' 
$num'' 
]'' 
.'' 
AppendChild'' (
(''( )
notificationXml'') 8
.''8 9
CreateTextNode''9 G
(''G H
title''H M
)''M N
)''N O
;''O P
toastElements(( 
[(( 
$num(( 
](( 
.(( 
AppendChild(( (
(((( )
notificationXml(() 8
.((8 9
CreateTextNode((9 G
(((G H
text((H L
)((L M
)((M N
;((N O
var** 
toast** 
=** 
new** 
ToastNotification** -
(**- .
notificationXml**. =
)**= >
{++ 
ExpirationTime,, 
=,,  
DateTime,,! )
.,,) *
Now,,* -
.,,- .

AddSeconds,,. 8
(,,8 9
$num,,9 :
),,: ;
}-- 
;-- $
ToastNotificationManager.. $
...$ %
CreateToastNotifier..% 8
(..8 9
)..9 :
...: ;
Show..; ?
(..? @
toast..@ E
)..E F
;..F G
}// 	
public11 
static11 
void11 
ShowErrorToast11 )
(11) *
	Exception11* 3
	exception114 =
)11= >
{22 	
ShowGenericToast33 
(33 
	exception33 &
.33& '
Source33' -
,33- .
	exception33/ 8
.338 9
Message339 @
)33@ A
;33A B
}44 	
}55 
}66 ç
`C:\Sources\Other\ModernKeePass\ModernKeePass\Converters\DiscreteIntToSolidColorBrushConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class 1
%DiscreteIntToSolidColorBrushConverter 6
:7 8
IValueConverter9 H
{		 
public

 
object

 
Convert

 
(

 
object

 $
value

% *
,

* +
Type

, 0

targetType

1 ;
,

; <
object

= C
	parameter

D M
,

M N
string

O U
language

V ^
)

^ _
{ 	
var 
status 
= 
System 
.  
Convert  '
.' (
ToInt32( /
(/ 0
value0 5
)5 6
;6 7
switch 
( 
status 
) 
{ 
case 
$num 
: 
return 
new "
SolidColorBrush# 2
(2 3
Colors3 9
.9 :
Red: =
)= >
;> ?
case 
$num 
: 
return 
new "
SolidColorBrush# 2
(2 3
Colors3 9
.9 :
Yellow: @
)@ A
;A B
case 
$num 
: 
return 
new "
SolidColorBrush# 2
(2 3
Colors3 9
.9 :
Green: ?
)? @
;@ A
default 
: 
return 
new  #
SolidColorBrush$ 3
(3 4
Colors4 :
.: ;
Black; @
)@ A
;A B
} 
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} ∂
[C:\Sources\Other\ModernKeePass\ModernKeePass\Converters\EmptyStringToVisibilityConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
class 	,
 EmptyStringToVisibilityConverter
 *
:* +
IValueConverter, ;
{ 
public		 
object		 
Convert		 
(		 
object		 $
value		% *
,		* +
Type		, 0

targetType		1 ;
,		; <
object		= C
	parameter		D M
,		M N
string		O U
language		V ^
)		^ _
{

 	
var 
text 
= 
value 
is 
string  &
?' (
value) .
.. /
ToString/ 7
(7 8
)8 9
:: ;
string< B
.B C
EmptyC H
;H I
return 
string 
. 
IsNullOrEmpty '
(' (
text( ,
), -
?. /

Visibility0 :
.: ;
	Collapsed; D
:E F

VisibilityG Q
.Q R
VisibleR Y
;Y Z
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} √	
QC:\Sources\Other\ModernKeePass\ModernKeePass\Converters\NullToBooleanConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class "
NullToBooleanConverter '
:( )
IValueConverter* 9
{ 
public 
object 
Convert 
( 
object $
value% *
,* +
Type, 0

targetType1 ;
,; <
object= C
	parameterD M
,M N
stringO U
languageV ^
)^ _
{		 	
return

 
value

 
!=

 
null

  
;

  !
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} ¡
HC:\Sources\Other\ModernKeePass\ModernKeePass\Exceptions\SaveException.cs
	namespace 	
ModernKeePass
 
. 

Exceptions "
{ 
public 

class 
SaveException 
:  
	Exception! *
{ 
public 
new 
	Exception 
InnerException +
{, -
get. 1
;1 2
}3 4
public		 
SaveException		 
(		 
	Exception		 &
	exception		' 0
)		0 1
{

 	
InnerException 
= 
	exception &
;& '
} 	
} 
} œ
SC:\Sources\Other\ModernKeePass\ModernKeePass\Extensions\DispatcherTaskExtensions.cs
	namespace 	
ModernKeePass
 
. 

Extensions "
{ 
public 

static 
class $
DispatcherTaskExtensions 0
{ 
public		 
static		 
async		 
Task		  
<		  !
T		! "
>		" #
RunTaskAsync		$ 0
<		0 1
T		1 2
>		2 3
(		3 4
this		4 8
CoreDispatcher		9 G

dispatcher		H R
,		R S
Func

 
<

 
Task

 
<

 
T

 
>

 
>

 
func

 
,

 "
CoreDispatcherPriority

  6
priority

7 ?
=

@ A"
CoreDispatcherPriority

B X
.

X Y
Normal

Y _
)

_ `
{ 	
var  
taskCompletionSource $
=% &
new' * 
TaskCompletionSource+ ?
<? @
T@ A
>A B
(B C
)C D
;D E
await 

dispatcher 
. 
RunAsync %
(% &
priority& .
,. /
async0 5
(6 7
)7 8
=>9 ;
{ 
try 
{  
taskCompletionSource (
.( )
	SetResult) 2
(2 3
await3 8
func9 =
(= >
)> ?
)? @
;@ A
} 
catch 
( 
	Exception  
ex! #
)# $
{  
taskCompletionSource (
.( )
SetException) 5
(5 6
ex6 8
)8 9
;9 :
} 
} 
) 
; 
return 
await  
taskCompletionSource -
.- .
Task. 2
;2 3
} 	
public 
static 
async 
Task  
RunTaskAsync! -
(- .
this. 2
CoreDispatcher3 A

dispatcherB L
,L M
Func 
< 
Task 
> 
func 
, "
CoreDispatcherPriority 3
priority4 <
== >"
CoreDispatcherPriority? U
.U V
NormalV \
)\ ]
=>^ `
await 
RunTaskAsync 
( 

dispatcher )
,) *
async+ 0
(1 2
)2 3
=>4 6
{7 8
await9 >
func? C
(C D
)D E
;E F
returnG M
falseN S
;S T
}U V
,V W
priorityX `
)` a
;a b
} 
}   „
KC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\IDatabaseService.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{		 
public

 

	interface

 
IDatabaseService

 %
{ 
string 
Name 
{ 
get 
; 
} 
bool 
RecycleBinEnabled 
{  
get! $
;$ %
set& )
;) *
}+ ,
GroupVm 
	RootGroup 
{ 
get 
;  
set! $
;$ %
}& '
GroupVm 

RecycleBin 
{ 
get  
;  !
set" %
;% &
}' (
StorageFile 
DatabaseFile  
{! "
get# &
;& '
set( +
;+ ,
}- .
CompositeKey 
CompositeKey !
{" #
get$ '
;' (
set) ,
;, -
}. /
PwUuid 

DataCipher 
{ 
get 
;  
set! $
;$ %
}& '"
PwCompressionAlgorithm  
CompressionAlgorithm 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
KdfParameters 
KeyDerivation #
{$ %
get& )
;) *
set+ .
;. /
}0 1
bool 
IsOpen 
{ 
get 
; 
} 
bool 

IsFileOpen 
{ 
get 
; 
}  
bool 
IsClosed 
{ 
get 
; 
} 
bool 

HasChanged 
{ 
get 
; 
set "
;" #
}$ %
Task 
Open 
( 
CompositeKey 
key "
," #
bool$ (
	createNew) 2
=3 4
false5 :
): ;
;; <
Task 
ReOpen 
( 
) 
; 
void 
Save 
( 
) 
; 
void 
Save 
( 
StorageFile 
file "
)" #
;# $
void 
CreateRecycleBin 
( 
string $
title% *
)* +
;+ ,
void   
AddDeletedItem   
(   
PwUuid   "
id  # %
)  % &
;  & '
Task!! 
Close!! 
(!! 
bool!! 
releaseFile!! #
=!!$ %
true!!& *
)!!* +
;!!+ ,
}"" 
}## √
OC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\IHasSelectableObject.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface  
IHasSelectableObject )
{ 
ISelectableModel 
SelectedItem %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
} 
} ≠
KC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\ISelectableModel.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface 
ISelectableModel %
{ 
bool 

IsSelected 
{ 
get 
; 
set "
;" #
}$ %
} 
} üI
SC:\Sources\Other\ModernKeePass\ModernKeePass\Views\BasePages\LayoutAwarePageBase.cs
	namespace		 	
ModernKeePass		
 
.		 
Views		 
.		 
	BasePages		 '
{

 
public 

class 
LayoutAwarePageBase $
:$ %
Page& *
{ 
public 
NavigationHelper 
NavigationHelper  0
{1 2
get3 6
;6 7
}8 9
public 
virtual 
ListView 
ListView  (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
virtual  
CollectionViewSource +
ListViewSource, :
{; <
get= @
;@ A
setB E
;E F
}G H
public 
virtual  
IHasSelectableObject +
Model, 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
public 
LayoutAwarePageBase "
(" #
)# $
{ 	
NavigationHelper 
= 
new "
NavigationHelper# 3
(3 4
this4 8
)8 9
;9 :
NavigationHelper 
. 
	LoadState &
+=' )&
navigationHelper_LoadState* D
;D E
NavigationHelper 
. 
	SaveState &
+=' )&
navigationHelper_SaveState* D
;D E
NavigationHelper   
.   
GoBackCommand   *
=  + ,
new  - 0
RelayCommand  1 =
(  = >
(  > ?
)  ? @
=>  A C
GoBack  D J
(  J K
)  K L
,  L M
(  N O
)  O P
=>  Q S
	CanGoBack  T ]
(  ] ^
)  ^ _
)  _ `
;  ` a
Window$$ 
.$$ 
Current$$ 
.$$ 
SizeChanged$$ &
+=$$' )
Window_SizeChanged$$* <
;$$< =!
InvalidateVisualState%% !
(%%! "
)%%" #
;%%# $
}&& 	
	protected(( 
void(( %
ListView_SelectionChanged(( 0
(((0 1
object((1 7
sender((8 >
,((> ?%
SelectionChangedEventArgs((@ Y
e((Z [
)(([ \
{)) 	
if// 
(// 
!// &
UsingLogicalPageNavigation// +
(//+ ,
)//, -
)//- .
return/// 5
;//5 6
NavigationHelper00 
.00 
GoBackCommand00 *
.00* +"
RaiseCanExecuteChanged00+ A
(00A B
)00B C
;00C D!
InvalidateVisualState11 !
(11! "
)11" #
;11# $
}22 	
	protected?? 
void?? &
navigationHelper_LoadState?? 1
(??1 2
object??2 8
sender??9 ?
,??? @
LoadStateEventArgs??A S
e??T U
)??U V
{@@ 	
ifDD 
(DD 
eDD 
.DD 
	PageStateDD 
==DD 
nullDD #
)DD# $
{EE 
ifHH 
(HH 
!HH &
UsingLogicalPageNavigationHH /
(HH/ 0
)HH0 1
)HH1 2
{II 
ListViewSourceJJ "
.JJ" #
ViewJJ# '
?JJ' (
.JJ( )
MoveCurrentToFirstJJ) ;
(JJ; <
)JJ< =
;JJ= >
}KK 
}LL 
elseMM 
{NN 
ifPP 
(PP 
ePP 
.PP 
	PageStatePP 
.PP  
ContainsKeyPP  +
(PP+ ,
$strPP, :
)PP: ;
)PP; <
{QQ 
ListViewSourceRR "
.RR" #
ViewRR# '
?RR' (
.RR( )
MoveCurrentToRR) 6
(RR6 7
eRR7 8
.RR8 9
	PageStateRR9 B
[RRB C
$strRRC Q
]RRQ R
)RRR S
;RRS T
}SS 
}TT 
}UU 	
	protected__ 
void__ &
navigationHelper_SaveState__ 1
(__1 2
object__2 8
sender__9 ?
,__? @
SaveStateEventArgs__A S
e__T U
)__U V
{`` 	
ifaa 
(aa 
ListViewSourceaa 
.aa 
Viewaa #
!=aa$ &
nullaa' +
)aa+ ,
{bb 
ecc 
.cc 
	PageStatecc 
[cc 
$strcc *
]cc* +
=cc, -
Modelcc. 3
?cc3 4
.cc4 5
SelectedItemcc5 A
;ccA B
}dd 
}ee 	
	protectedpp 
constpp 
intpp -
!MinimumWidthForSupportingTwoPanespp =
=pp> ?
$numpp@ C
;ppC D
	protectedww 
boolww &
UsingLogicalPageNavigationww 1
(ww1 2
)ww2 3
{xx 	
returnyy 
Windowyy 
.yy 
Currentyy !
.yy! "
Boundsyy" (
.yy( )
Widthyy) .
<yy/ 0-
!MinimumWidthForSupportingTwoPanesyy1 R
;yyR S
}zz 	
	protected
ÅÅ 
void
ÅÅ  
Window_SizeChanged
ÅÅ )
(
ÅÅ) *
object
ÅÅ* 0
sender
ÅÅ1 7
,
ÅÅ7 8
Windows
ÅÅ9 @
.
ÅÅ@ A
UI
ÅÅA C
.
ÅÅC D
Core
ÅÅD H
.
ÅÅH I(
WindowSizeChangedEventArgs
ÅÅI c
e
ÅÅd e
)
ÅÅe f
{
ÇÇ 	#
InvalidateVisualState
ÉÉ !
(
ÉÉ! "
)
ÉÉ" #
;
ÉÉ# $
}
ÑÑ 	
	protected
ÜÜ 
bool
ÜÜ 
	CanGoBack
ÜÜ  
(
ÜÜ  !
)
ÜÜ! "
{
áá 	
if
àà 
(
àà (
UsingLogicalPageNavigation
àà *
(
àà* +
)
àà+ ,
&&
àà- /
ListView
àà0 8
.
àà8 9
SelectedItem
àà9 E
!=
ààF H
null
ààI M
)
ààM N
{
ââ 
return
ää 
true
ää 
;
ää 
}
ãã 
return
åå 
NavigationHelper
åå #
.
åå# $
	CanGoBack
åå$ -
(
åå- .
)
åå. /
;
åå/ 0
}
çç 	
	protected
éé 
void
éé 
GoBack
éé 
(
éé 
)
éé 
{
èè 	
if
êê 
(
êê (
UsingLogicalPageNavigation
êê *
(
êê* +
)
êê+ ,
&&
êê- /
ListView
êê0 8
.
êê8 9
SelectedItem
êê9 E
!=
êêF H
null
êêI M
)
êêM N
{
ëë 
ListView
ññ 
.
ññ 
SelectedItem
ññ %
=
ññ& '
null
ññ( ,
;
ññ, -
}
óó 
else
òò 
{
ôô 
NavigationHelper
öö  
.
öö  !
GoBack
öö! '
(
öö' (
)
öö( )
;
öö) *
}
õõ 
}
úú 	
	protected
ûû 
void
ûû #
InvalidateVisualState
ûû ,
(
ûû, -
)
ûû- .
{
üü 	
var
†† 
visualState
†† 
=
†† "
DetermineVisualState
†† 2
(
††2 3
)
††3 4
;
††4 5 
VisualStateManager
°° 
.
°° 
	GoToState
°° (
(
°°( )
this
°°) -
,
°°- .
visualState
°°/ :
,
°°: ;
false
°°< A
)
°°A B
;
°°B C
NavigationHelper
¢¢ 
.
¢¢ 
GoBackCommand
¢¢ *
.
¢¢* +$
RaiseCanExecuteChanged
¢¢+ A
(
¢¢A B
)
¢¢B C
;
¢¢C D
}
££ 	
	protected
¨¨ 
string
¨¨ "
DetermineVisualState
¨¨ -
(
¨¨- .
)
¨¨. /
{
≠≠ 	
if
ÆÆ 
(
ÆÆ 
!
ÆÆ (
UsingLogicalPageNavigation
ÆÆ +
(
ÆÆ+ ,
)
ÆÆ, -
)
ÆÆ- .
return
ØØ 
$str
ØØ $
;
ØØ$ %
var
≤≤ 
logicalPageBack
≤≤ 
=
≤≤  !(
UsingLogicalPageNavigation
≤≤" <
(
≤≤< =
)
≤≤= >
&&
≤≤? A
ListView
≤≤B J
?
≤≤J K
.
≤≤K L
SelectedItem
≤≤L X
!=
≤≤Y [
null
≤≤\ `
;
≤≤` a
return
¥¥ 
logicalPageBack
¥¥ "
?
¥¥# $
$str
¥¥% 8
:
¥¥9 :
$str
¥¥; G
;
¥¥G H
}
µµ 	
	protected
ƒƒ 
override
ƒƒ 
void
ƒƒ 
OnNavigatedTo
ƒƒ  -
(
ƒƒ- .!
NavigationEventArgs
ƒƒ. A
e
ƒƒB C
)
ƒƒC D
{
≈≈ 	
NavigationHelper
∆∆ 
.
∆∆ 
OnNavigatedTo
∆∆ *
(
∆∆* +
e
∆∆+ ,
)
∆∆, -
;
∆∆- .
}
«« 	
	protected
…… 
override
…… 
void
…… 
OnNavigatedFrom
……  /
(
……/ 0!
NavigationEventArgs
……0 C
e
……D E
)
……E F
{
   	
NavigationHelper
ÀÀ 
.
ÀÀ 
OnNavigatedFrom
ÀÀ ,
(
ÀÀ, -
e
ÀÀ- .
)
ÀÀ. /
;
ÀÀ/ 0
}
ÃÃ 	
}
œœ 
}–– †
bC:\Sources\Other\ModernKeePass\ModernKeePass\Views\SettingsPageFrames\SettingsDatabasePage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class  
SettingsDatabasePage  4
{		 
public

  
SettingsDatabasePage

 #
(

# $
)

$ %
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} ©
eC:\Sources\Other\ModernKeePass\ModernKeePass\Views\SettingsPageFrames\SettingsNewDatabasePage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class #
SettingsNewDatabasePage  7
{		 
public

 #
SettingsNewDatabasePage

 &
(

& '
)

' (
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} î
^C:\Sources\Other\ModernKeePass\ModernKeePass\Views\SettingsPageFrames\SettingsSavePage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
SettingsSavePage  0
{		 
public

 
SettingsSavePage

 
(

  
)

  !
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} ”
bC:\Sources\Other\ModernKeePass\ModernKeePass\Views\SettingsPageFrames\SettingsSecurityPage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class  
SettingsSecurityPage  4
{ 
public  
SettingsSecurityPage #
(# $
)$ %
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
void 7
+CompositeKeyUserControl_OnValidationChecked @
(@ A
objectA G
senderH N
,N O
PasswordEventArgsP a
eb c
)c d
{ 	#
ToastNotificationHelper #
.# $
ShowGenericToast$ 4
(4 5
$str5 D
,D E
$strF f
)f g
;g h
} 	
} 
} ù
aC:\Sources\Other\ModernKeePass\ModernKeePass\Views\SettingsPageFrames\SettingsWelcomePage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
SettingsWelcomePage  3
{		 
public

 
SettingsWelcomePage

 "
(

" #
)

# $
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} §
_C:\Sources\Other\ModernKeePass\ModernKeePass\TemplateSelectors\FirstItemDataTemplateSelector.cs
	namespace 	
ModernKeePass
 
. 
TemplateSelectors )
{ 
public 

class )
FirstItemDataTemplateSelector .
:. / 
DataTemplateSelector0 D
{ 
public 
DataTemplate 
	FirstItem %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public		 
DataTemplate		 
	OtherItem		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		2 3
	protected 
override 
DataTemplate '
SelectTemplateCore( :
(: ;
object; A
itemB F
,F G
DependencyObjectH X
	containerY b
)b c
{ 	
var 
itemsControl 
= 
ItemsControl +
.+ ,)
ItemsControlFromItemContainer, I
(I J
	containerJ S
)S T
;T U
var 
returnTemplate 
=  
itemsControl! -
?- .
.. /
IndexFromContainer/ A
(A B
	containerB K
)K L
==M O
$numP Q
?R S
	FirstItemT ]
:^ _
	OtherItem` i
;i j
return 
returnTemplate !
;! "
} 	
} 
} ú
LC:\Sources\Other\ModernKeePass\ModernKeePass\Controls\ListViewWithDisable.cs
	namespace 	
ModernKeePass
 
. 
Controls  
{ 
public 

class 
ListViewWithDisable $
:$ %
ListView& .
{ 
	protected		 
override		 
void		 +
PrepareContainerForItemOverride		  ?
(		? @
DependencyObject		@ P
element		Q X
,		X Y
object		Z `
item		a e
)		e f
{

 	
base 
. +
PrepareContainerForItemOverride 0
(0 1
element1 8
,8 9
item: >
)> ?
;? @
var 
	container 
= 
element #
as$ &
ListViewItem' 3
;3 4
var 

binaryItem 
= 
item !
as" $

IIsEnabled% /
;/ 0
if 
( 
	container 
== 
null !
||" $

binaryItem% /
==0 2
null3 7
)7 8
return9 ?
;? @
	container 
. 
	IsEnabled 
=  !

binaryItem" ,
., -
	IsEnabled- 6
;6 7
	container 
. 
IsHitTestVisible &
=' (

binaryItem) 3
.3 4
	IsEnabled4 =
;= >
} 	
} 
} ä
]C:\Sources\Other\ModernKeePass\ModernKeePass\Views\UserControls\BreadCrumbUserControl.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
. 
UserControls *
{ 
public		 

sealed		 
partial		 
class		 !
BreadCrumbUserControl		  5
{

 
public !
BreadCrumbUserControl $
($ %
)% &
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
public 
IEnumerable 
< 
	IPwEntity $
>$ %
ItemsSource& 1
{ 	
get 
{ 
return 
( 
IEnumerable %
<% &
	IPwEntity& /
>/ 0
)0 1
GetValue1 9
(9 :
ItemsSourceProperty: M
)M N
;N O
}P Q
set 
{ 
SetValue 
( 
ItemsSourceProperty .
,. /
value0 5
)5 6
;6 7
}8 9
} 	
public 
static 
readonly 
DependencyProperty 1
ItemsSourceProperty2 E
=F G
DependencyProperty 
. 
Register '
(' (
$str 
, 
typeof 
( 
IEnumerable "
<" #
	IPwEntity# ,
>, -
)- .
,. /
typeof 
( !
BreadCrumbUserControl ,
), -
,- .
new 
PropertyMetadata $
($ %
new% (
Stack) .
<. /
	IPwEntity/ 8
>8 9
(9 :
): ;
,; <
(= >
o> ?
,? @
argsA E
)E F
=>G I
{J K
}L M
)M N
)N O
;O P
} 
} †T
_C:\Sources\Other\ModernKeePass\ModernKeePass\Views\UserControls\CompositeKeyUserControl.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
. 
UserControls *
{ 
public 

sealed 
partial 
class #
CompositeKeyUserControl  7
{ 
public 
CompositeKeyVm 
Model #
=>$ &
Grid' +
.+ ,
DataContext, 7
as8 :
CompositeKeyVm; I
;I J
public 
bool 
	CreateNew 
{ 	
get 
{ 
return 
( 
bool 
) 
GetValue '
(' (
CreateNewProperty( 9
)9 :
;: ;
}< =
set 
{ 
SetValue 
( 
CreateNewProperty ,
,, -
value. 3
)3 4
;4 5
}6 7
} 	
public 
static 
readonly 
DependencyProperty 1
CreateNewProperty2 C
=D E
DependencyProperty 
. 
Register '
(' (
$str 
, 
typeof 
( 
bool 
) 
, 
typeof 
( #
CompositeKeyUserControl .
). /
,/ 0
new 
PropertyMetadata $
($ %
false% *
,* +
(, -
o- .
,. /
args0 4
)4 5
=>6 8
{9 :
}; <
)< =
)= >
;> ?
public   
bool   
	UpdateKey   
{!! 	
get"" 
{"" 
return"" 
("" 
bool"" 
)"" 
GetValue"" '
(""' (
UpdateKeyProperty""( 9
)""9 :
;"": ;
}""< =
set## 
{## 
SetValue## 
(## 
UpdateKeyProperty## ,
,##, -
value##. 3
)##3 4
;##4 5
}##6 7
}$$ 	
public%% 
static%% 
readonly%% 
DependencyProperty%% 1
UpdateKeyProperty%%2 C
=%%D E
DependencyProperty&& 
.&& 
Register&& '
(&&' (
$str'' 
,'' 
typeof(( 
((( 
bool(( 
)(( 
,(( 
typeof)) 
()) #
CompositeKeyUserControl)) .
))). /
,))/ 0
new** 
PropertyMetadata** $
(**$ %
false**% *
,*** +
(**, -
o**- .
,**. /
args**0 4
)**4 5
=>**6 8
{**9 :
}**; <
)**< =
)**= >
;**> ?
public,, 
string,, 
ButtonLabel,, !
{-- 	
get.. 
{.. 
return.. 
(.. 
string..  
)..  !
GetValue..! )
(..) *
ButtonLabelProperty..* =
)..= >
;..> ?
}..@ A
set// 
{// 
SetValue// 
(// 
ButtonLabelProperty// .
,//. /
value//0 5
)//5 6
;//6 7
}//8 9
}00 	
public11 
static11 
readonly11 
DependencyProperty11 1
ButtonLabelProperty112 E
=11F G
DependencyProperty22 
.22 
Register22 '
(22' (
$str33 
,33 
typeof44 
(44 
string44 
)44 
,44 
typeof55 
(55 #
CompositeKeyUserControl55 .
)55. /
,55/ 0
new66 
PropertyMetadata66 $
(66$ %
$str66% )
,66) *
(66+ ,
o66, -
,66- .
args66/ 3
)663 4
=>665 7
{668 9
}66: ;
)66; <
)66< =
;66= >
public99 
bool99 #
ShowComplexityIndicator99 +
=>99, .
	CreateNew99/ 8
||999 ;
	UpdateKey99< E
;99E F
public;; #
CompositeKeyUserControl;; &
(;;& '
);;' (
{<< 	
InitializeComponent== 
(==  
)==  !
;==! "
}>> 	
public@@ 
event@@ (
PasswordCheckingEventHandler@@ 1
ValidationChecking@@2 D
;@@D E
publicAA 
delegateAA 
voidAA (
PasswordCheckingEventHandlerAA 9
(AA9 :
objectAA: @
senderAAA G
,AAG H
	EventArgsAAI R
eAAS T
)AAT U
;AAU V
publicBB 
eventBB '
PasswordCheckedEventHandlerBB 0
ValidationCheckedBB1 B
;BBB C
publicCC 
delegateCC 
voidCC '
PasswordCheckedEventHandlerCC 8
(CC8 9
objectCC9 ?
senderCC@ F
,CCF G
PasswordEventArgsCCH Y
eCCZ [
)CC[ \
;CC\ ]
privateEE 
asyncEE 
voidEE 
OpenButton_OnClickEE -
(EE- .
objectEE. 4
senderEE5 ;
,EE; <
RoutedEventArgsEE= L
eEEM N
)EEN O
{FF 	
ValidationCheckingGG 
?GG 
.GG  
InvokeGG  &
(GG& '
thisGG' +
,GG+ ,
newGG- 0
	EventArgsGG1 :
(GG: ;
)GG; <
)GG< =
;GG= >
ifII 
(II 
	UpdateKeyII 
)II 
{JJ 
ModelKK 
.KK 
	UpdateKeyKK 
(KK  
)KK  !
;KK! "
ValidationCheckedLL !
?LL! "
.LL" #
InvokeLL# )
(LL) *
thisLL* .
,LL. /
newLL0 3
PasswordEventArgsLL4 E
(LLE F
ModelLLF K
.LLK L
	RootGroupLLL U
)LLU V
)LLV W
;LLW X
}MM 
elseNN 
{OO 
varPP 
resourcePP 
=PP 
newPP "
ResourcesServicePP# 3
(PP3 4
)PP4 5
;PP5 6
varQQ 
oldLabelQQ 
=QQ 
ButtonLabelQQ *
;QQ* +
ButtonLabelRR 
=RR 
resourceRR &
.RR& '
GetResourceValueRR' 7
(RR7 8
$strRR8 M
)RRM N
;RRN O
ifSS 
(SS 
awaitSS 

DispatcherSS $
.SS$ %
RunTaskAsyncSS% 1
(SS1 2
asyncSS2 7
(SS8 9
)SS9 :
=>SS; =
awaitSS> C
ModelSSD I
.SSI J
OpenDatabaseSSJ V
(SSV W
	CreateNewSSW `
)SS` a
)SSa b
)SSb c
{TT 
ValidationCheckedUU %
?UU% &
.UU& '
InvokeUU' -
(UU- .
thisUU. 2
,UU2 3
newUU4 7
PasswordEventArgsUU8 I
(UUI J
ModelUUJ O
.UUO P
	RootGroupUUP Y
)UUY Z
)UUZ [
;UU[ \
}VV 
ButtonLabelWW 
=WW 
oldLabelWW &
;WW& '
}XX 
}YY 	
private[[ 
void[[ 
PasswordBox_KeyDown[[ (
([[( )
object[[) /
sender[[0 6
,[[6 7
KeyRoutedEventArgs[[8 J
e[[K L
)[[L M
{\\ 	
if]] 
(]] 
e]] 
.]] 
Key]] 
==]] 

VirtualKey]] #
.]]# $
Enter]]$ )
&&]]* ,
Model]]- 2
.]]2 3
IsValid]]3 :
)]]: ;
{^^ 
OpenButton_OnClick__ "
(__" #
sender__# )
,__) *
e__+ ,
)__, -
;__- .
eaa 
.aa 
Handledaa 
=aa 
trueaa  
;aa  !
}bb 
}cc 	
privateee 
asyncee 
voidee 
KeyFileButton_Clickee .
(ee. /
objectee/ 5
senderee6 <
,ee< =
RoutedEventArgsee> M
eeeN O
)eeO P
{ff 	
vargg 
pickergg 
=gg 
newhh 
FileOpenPickerhh "
{ii 
ViewModejj 
=jj 
PickerViewModejj -
.jj- .
Listjj. 2
,jj2 3"
SuggestedStartLocationkk *
=kk+ ,
PickerLocationIdkk- =
.kk= >
DocumentsLibrarykk> N
}ll 
;ll 
pickermm 
.mm 
FileTypeFiltermm !
.mm! "
Addmm" %
(mm% &
$strmm& ,
)mm, -
;mm- .
varpp 
filepp 
=pp 
awaitpp 
pickerpp #
.pp# $
PickSingleFileAsyncpp$ 7
(pp7 8
)pp8 9
;pp9 :
ifqq 
(qq 
fileqq 
==qq 
nullqq 
)qq 
returnqq $
;qq$ %
Modelrr 
.rr 
KeyFilerr 
=rr 
filerr  
;rr  !
}ss 	
privateuu 
asyncuu 
voiduu %
CreateKeyFileButton_Clickuu 4
(uu4 5
objectuu5 ;
senderuu< B
,uuB C
RoutedEventArgsuuD S
euuT U
)uuU V
{vv 	
varww 

savePickerww 
=ww 
newww  
FileSavePickerww! /
{xx "
SuggestedStartLocationyy &
=yy' (
PickerLocationIdyy) 9
.yy9 :
DocumentsLibraryyy: J
,yyJ K
SuggestedFileNamezz !
=zz" #
$strzz$ )
}{{ 
;{{ 

savePicker|| 
.|| 
FileTypeChoices|| &
.||& '
Add||' *
(||* +
$str||+ 5
,||5 6
new||7 :
List||; ?
<||? @
string||@ F
>||F G
{||H I
$str||J P
}||Q R
)||R S
;||S T
var~~ 
file~~ 
=~~ 
await~~ 

savePicker~~ '
.~~' (
PickSaveFileAsync~~( 9
(~~9 :
)~~: ;
;~~; <
if 
( 
file 
== 
null 
) 
return $
;$ %
Model
ÅÅ 
.
ÅÅ 
CreateKeyFile
ÅÅ 
(
ÅÅ  
file
ÅÅ  $
)
ÅÅ$ %
;
ÅÅ% &
}
ÇÇ 	
}
ÉÉ 
}ÑÑ Å1
JC:\Sources\Other\ModernKeePass\ModernKeePass\Controls\TextBoxWithButton.cs
	namespace 	
ModernKeePass
 
. 
Controls  
{ 
public 

class 
TextBoxWithButton "
:# $
TextBox% ,
{		 
public

 
string

 
ButtonSymbol

 "
{ 	
get 
{ 
return 
( 
string  
)  !
GetValue! )
() * 
ButtonSymbolProperty* >
)> ?
;? @
}A B
set 
{ 
SetValue 
(  
ButtonSymbolProperty /
,/ 0
value1 6
)6 7
;7 8
}9 :
} 	
public 
static 
readonly 
DependencyProperty 1 
ButtonSymbolProperty2 F
=G H
DependencyProperty 
. 
Register '
(' (
$str 
, 
typeof 
( 
string 
) 
, 
typeof 
( 
TextBoxWithButton (
)( )
,) *
new 
PropertyMetadata $
($ %
$str% /
,/ 0
(1 2
o2 3
,3 4
args5 9
)9 :
=>; =
{> ?
}@ A
)A B
)B C
;C D
public 
event 
EventHandler !
<! "
RoutedEventArgs" 1
>1 2
ButtonClick3 >
;> ?
public 
string 
ButtonTooltip #
{ 	
get 
{ 
return 
( 
string  
)  !
GetValue! )
() *!
ButtonTooltipProperty* ?
)? @
;@ A
}B C
set 
{ 
SetValue 
( !
ButtonTooltipProperty 0
,0 1
value2 7
)7 8
;8 9
}: ;
} 	
public 
static 
readonly 
DependencyProperty 1!
ButtonTooltipProperty2 G
=H I
DependencyProperty 
. 
Register '
(' (
$str 
,  
typeof 
( 
string 
) 
, 
typeof   
(   
TextBoxWithButton   (
)  ( )
,  ) *
new!! 
PropertyMetadata!! $
(!!$ %
string!!% +
.!!+ ,
Empty!!, 1
,!!1 2
(!!3 4
o!!4 5
,!!5 6
args!!7 ;
)!!; <
=>!!= ?
{!!@ A
}!!B C
)!!C D
)!!D E
;!!E F
public## 
bool## 
IsButtonEnabled## #
{$$ 	
get%% 
{%% 
return%% 
(%% 
bool%% 
)%% 
GetValue%% '
(%%' (#
IsButtonEnabledProperty%%( ?
)%%? @
;%%@ A
}%%B C
set&& 
{&& 
SetValue&& 
(&& #
IsButtonEnabledProperty&& 2
,&&2 3
value&&4 9
)&&9 :
;&&: ;
}&&< =
}'' 	
public(( 
static(( 
readonly(( 
DependencyProperty(( 1#
IsButtonEnabledProperty((2 I
=((J K
DependencyProperty)) 
.)) 
Register)) '
())' (
$str** !
,**! "
typeof++ 
(++ 
bool++ 
)++ 
,++ 
typeof,, 
(,, 
TextBoxWithButton,, (
),,( )
,,,) *
new-- 
PropertyMetadata-- $
(--$ %
true--% )
,--) *
(--+ ,
o--, -
,--- .
args--/ 3
)--3 4
=>--5 7
{--8 9
}--: ;
)--; <
)--< =
;--= >
public// 

FlyoutBase// 
ButtonFlyout// &
{00 	
get11 
{11 
return11 
(11 

FlyoutBase11 $
)11$ %
GetValue11% -
(11- . 
ButtonFlyoutProperty11. B
)11B C
;11C D
}11E F
set22 
{22 
SetValue22 
(22  
ButtonFlyoutProperty22 /
,22/ 0
value221 6
)226 7
;227 8
}229 :
}33 	
public44 
static44 
readonly44 
DependencyProperty44 1 
ButtonFlyoutProperty442 F
=44G H
DependencyProperty55 
.55 
Register55 '
(55' (
$str66 
,66 
typeof77 
(77 

FlyoutBase77 !
)77! "
,77" #
typeof88 
(88 
TextBoxWithButton88 (
)88( )
,88) *
new99 
PropertyMetadata99 $
(99$ %
null99% )
,99) *
(99+ ,
o99, -
,99- .
args99/ 3
)993 4
=>995 7
{998 9
}99: ;
)99; <
)99< =
;99= >
	protected;; 
override;; 
void;; 
OnApplyTemplate;;  /
(;;/ 0
);;0 1
{<< 	
base== 
.== 
OnApplyTemplate==  
(==  !
)==! "
;==" #
var>> 
actionButton>> 
=>> 
GetTemplateChild>> /
(>>/ 0
$str>>0 >
)>>> ?
as>>@ B
Button>>C I
;>>I J
if?? 
(?? 
actionButton?? 
!=?? 
null??  $
)??$ %
{@@ 
actionButtonAA 
.AA 
ClickAA "
+=AA# %
(AA& '
senderAA' -
,AA- .
eAA/ 0
)AA0 1
=>AA2 4
ButtonClickAA5 @
?AA@ A
.AAA B
InvokeAAB H
(AAH I
senderAAI O
,AAO P
eAAQ R
)AAR S
;AAS T
}BB 
}CC 	
}DD 
}EE ê
WC:\Sources\Other\ModernKeePass\ModernKeePass\Converters\BooleanToVisibilityConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class (
BooleanToVisibilityConverter -
:. /
IValueConverter0 ?
{ 
public		 
object		 
Convert		 
(		 
object		 $
value		% *
,		* +
Type		, 0

targetType		1 ;
,		; <
object		= C
	parameter		D M
,		M N
string		O U
language		V ^
)		^ _
{

 	
var 
boolean 
= 
value 
is  "
bool# '
?( )
(* +
bool+ /
)/ 0
value1 6
:7 8
false9 >
;> ?
return 
boolean 
? 

Visibility '
.' (
Visible( /
:0 1

Visibility2 <
.< =
	Collapsed= F
;F G
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
var 

visibility 
= 
value "
is# %

Visibility& 0
?1 2
(3 4

Visibility4 >
)> ?
value@ E
:F G

VisibilityH R
.R S
VisibleS Z
;Z [
switch 
( 

visibility 
) 
{ 
case 

Visibility 
.  
Visible  '
:' (
return) /
true0 4
;4 5
case 

Visibility 
.  
	Collapsed  )
:) *
return+ 1
false2 7
;7 8
default 
: 
throw 
new '
ArgumentOutOfRangeException 9
(9 :
): ;
;; <
} 
} 	
} 
} ◊
PC:\Sources\Other\ModernKeePass\ModernKeePass\Converters\ColorToBrushConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class !
ColorToBrushConverter &
:' (
IValueConverter) 8
{		 
public

 
object

 
Convert

 
(

 
object

 $
value

% *
,

* +
Type

, 0

targetType

1 ;
,

; <
object

= C
	parameter

D M
,

M N
string

O U
language

V ^
)

^ _
{ 	
var 
color 
= 
value 
is  
Color! &
?' (
() *
Color* /
?/ 0
)0 1
value2 7
:8 9
Color: ?
.? @
Empty@ E
;E F
if 
( 
color 
== 
Color 
. 
Empty $
&&% '
	parameter( 1
is2 4
SolidColorBrush5 D
)D E
returnF L
(M N
SolidColorBrushN ]
)] ^
	parameter_ h
;h i
return 
new 
SolidColorBrush &
(& '
Windows' .
.. /
UI/ 1
.1 2
Color2 7
.7 8
FromArgb8 @
(@ A
color 
. 
Value 
. 
A 
, 
color 
. 
Value 
. 
R 
, 
color 
. 
Value 
. 
G 
, 
color 
. 
Value 
. 
B 
) 
) 
;  
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} æ
[C:\Sources\Other\ModernKeePass\ModernKeePass\Converters\DoubleToSolidColorBrushConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class ,
 DoubleToSolidColorBrushConverter 1
:2 3
IValueConverter4 C
{		 
public

 
object

 
Convert

 
(

 
object

 $
value

% *
,

* +
Type

, 0

targetType

1 ;
,

; <
object

= C
	parameter

D M
,

M N
string

O U
language

V ^
)

^ _
{ 	
try 
{ 
var 
currentValue  
=! "
(# $
double$ *
)* +
value, 1
;1 2
var 
maxValue 
= 
double %
.% &
Parse& +
(+ ,
	parameter, 5
as6 8
string9 ?
)? @
;@ A
var 
green 
= 
System "
." #
Convert# *
.* +
ToByte+ 1
(1 2
currentValue2 >
/? @
maxValueA I
*J K
byteL P
.P Q
MaxValueQ Y
)Y Z
;Z [
var 
red 
= 
( 
byte 
)  
(! "
byte" &
.& '
MaxValue' /
-0 1
green2 7
)7 8
;8 9
return 
new 
SolidColorBrush *
(* +
Color+ 0
.0 1
FromArgb1 9
(9 :
$num: =
,= >
red? B
,B C
greenD I
,I J
$numK L
)L M
)M N
;N O
} 
catch 
( 
OverflowException $
)$ %
{ 
return 
new 
SolidColorBrush *
(* +
Color+ 0
.0 1
FromArgb1 9
(9 :
$num: =
,= >
$num? @
,@ A
byteB F
.F G
MaxValueG O
,O P
$numQ R
)R S
)S T
;T U
} 
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} û
^C:\Sources\Other\ModernKeePass\ModernKeePass\Converters\InverseBooleanToVisibilityConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class /
#InverseBooleanToVisibilityConverter 4
:5 6
IValueConverter7 F
{ 
public		 
object		 
Convert		 
(		 
object		 $
value		% *
,		* +
Type		, 0

targetType		1 ;
,		; <
object		= C
	parameter		D M
,		M N
string		O U
language		V ^
)		^ _
{

 	
var 
boolean 
= 
value 
is  "
bool# '
?( )
(* +
bool+ /
)/ 0
value0 5
:6 7
false8 =
;= >
return 
boolean 
? 

Visibility '
.' (
	Collapsed( 1
:2 3

Visibility4 >
.> ?
Visible? F
;F G
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
var 

visibility 
= 
value "
is# %

Visibility& 0
?1 2
(3 4

Visibility4 >
)> ?
value? D
:E F

VisibilityG Q
.Q R
VisibleR Y
;Y Z
switch 
( 

visibility 
) 
{ 
case 

Visibility 
.  
Visible  '
:' (
return) /
false0 5
;5 6
case 

Visibility 
.  
	Collapsed  )
:) *
return+ 1
true2 6
;6 7
default 
: 
throw 
new '
ArgumentOutOfRangeException 9
(9 :
): ;
;; <
} 
} 	
} 
} Ö
QC:\Sources\Other\ModernKeePass\ModernKeePass\Converters\PluralizationConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class "
PluralizationConverter '
:( )
IValueConverter* 9
{ 
public 
object 
Convert 
( 
object $
value% *
,* +
Type, 0

targetType1 ;
,; <
object= C
	parameterD M
,M N
stringO U
languageV ^
)^ _
{		 	
var

 %
pluralizationOptionString

 )
=

* +
	parameter

, 5
as

6 8
string

9 ?
;

? @
var  
pluralizationOptions $
=% &%
pluralizationOptionString' @
?@ A
.A B
SplitB G
(G H
newH K
[K L
]L M
{N O
$strP S
}T U
,U V
StringSplitOptionsW i
.i j
RemoveEmptyEntriesj |
)| }
;} ~
if 
(  
pluralizationOptions $
==% '
null( ,
||- / 
pluralizationOptions0 D
.D E
LengthE K
!=L N
$numO P
)P Q
returnR X
stringY _
._ `
Empty` e
;e f
var 
count 
= 
value 
is  
int! $
?% &
(' (
int( +
)+ ,
value- 2
:3 4
$num5 6
;6 7
var 
text 
= 
count 
== 
$num  !
?" # 
pluralizationOptions$ 8
[8 9
$num9 :
]: ;
:< = 
pluralizationOptions> R
[R S
$numS T
]T U
;U V
return 
$" 
{ 
count 
} 
{ 
text "
}" #
"# $
;$ %
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} ⁄
ZC:\Sources\Other\ModernKeePass\ModernKeePass\Converters\ProgressBarLegalValuesConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class +
ProgressBarLegalValuesConverter 0
:1 2
IValueConverter3 B
{ 
public 
object 
Convert 
( 
object $
value% *
,* +
Type, 0

targetType1 ;
,; <
object= C
	parameterD M
,M N
stringO U
languageV ^
)^ _
{		 	
var

 #
legalValuesOptionString

 '
=

( )
	parameter

* 3
as

4 6
string

7 =
;

= >
var 
legalValuesOptions "
=# $#
legalValuesOptionString% <
?< =
.= >
Split> C
(C D
newD G
[G H
]H I
{J K
$strL O
}P Q
,Q R
StringSplitOptionsS e
.e f
RemoveEmptyEntriesf x
)x y
;y z
if 
( 
legalValuesOptions "
==# %
null& *
||+ -
legalValuesOptions. @
.@ A
LengthA G
!=H J
$numK L
)L M
returnN T
$numU V
;V W
var 
minValue 
= 
double !
.! "
Parse" '
(' (
legalValuesOptions( :
[: ;
$num; <
]< =
)= >
;> ?
var 
maxValue 
= 
double !
.! "
Parse" '
(' (
legalValuesOptions( :
[: ;
$num; <
]< =
)= >
;> ?
var 
count 
= 
value 
is  
double! '
?( )
(* +
double+ 1
)1 2
value2 7
:8 9
$num: ;
;; <
if 
( 
count 
> 
maxValue  
)  !
return" (
maxValue) 1
;1 2
if 
( 
count 
< 
minValue  
)  !
return" (
minValue) 1
;1 2
return 
count 
; 
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} ·
OC:\Sources\Other\ModernKeePass\ModernKeePass\Converters\TextToWidthConverter.cs
	namespace 	
ModernKeePass
 
. 

Converters "
{ 
public 

class  
TextToWidthConverter %
:& '
IValueConverter( 7
{ 
public 
object 
Convert 
( 
object $
value% *
,* +
Type, 0

targetType1 ;
,; <
object= C
	parameterD M
,M N
stringO U
languageV ^
)^ _
{		 	
var

 
fontSize

 
=

 
double

 !
.

! "
Parse

" '
(

' (
	parameter

( 1
as

2 4
string

5 ;
)

; <
;

< =
var 
text 
= 
value 
as 
string  &
;& '
return 
text 
? 
. 
Length 
*  !
fontSize" *
??+ -
$num. /
;/ 0
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
stringS Y
languageZ b
)b c
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} ¬
HC:\Sources\Other\ModernKeePass\ModernKeePass\Events\PasswordEventArgs.cs
	namespace 	
ModernKeePass
 
. 
Events 
{ 
public 

class 
PasswordEventArgs "
:" #
	EventArgs$ -
{ 
public 
GroupVm 
	RootGroup  
{! "
get# &
;& '
set( +
;+ ,
}- .
public

 
PasswordEventArgs

  
(

  !
GroupVm

! (
groupVm

) 0
)

0 1
{ 	
	RootGroup 
= 
groupVm 
;  
} 	
} 
} Ä
EC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\IIsEnabled.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface 

IIsEnabled 
{ 
bool 
	IsEnabled 
{ 
get 
; 
} 
} 
} ¿
DC:\Sources\Other\ModernKeePass\ModernKeePass\Interfaces\IPwEntity.cs
	namespace 	
ModernKeePass
 
. 

Interfaces "
{ 
public 

	interface 
	IPwEntity 
{ 
GroupVm 
ParentGroup 
{ 
get !
;! "
}# $
GroupVm		 
PreviousGroup		 
{		 
get		  #
;		# $
}		% &
int

 
IconId

 
{

 
get

 
;

 
}

 
string 
Id 
{ 
get 
; 
} 
string 
Name 
{ 
get 
; 
set 
; 
}  !
IEnumerable 
< 
	IPwEntity 
> 

BreadCrumb )
{* +
get, /
;/ 0
}1 2
bool 

IsEditMode 
{ 
get 
; 
}  
bool 
IsRecycleOnDelete 
{  
get! $
;$ %
}& '
void 
Move 
( 
GroupVm 
destination %
)% &
;& '
void 
CommitDelete 
( 
) 
; 
void 

UndoDelete 
( 
) 
; 
void!! 
Save!! 
(!! 
)!! 
;!! 
void%% 
MarkForDelete%% 
(%% 
string%% !
recycleBinTitle%%" 1
)%%1 2
;%%2 3
}&& 
}'' Á
CC:\Sources\Other\ModernKeePass\ModernKeePass\Views\MainPage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
MainPage  (
{ 
public 
new 
MainVm 
Model 
=>  "
(# $
MainVm$ *
)* +
DataContext+ 6
;6 7
public 
MainPage 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
ListView 
= 
MenuListView #
;# $
ListViewSource 
= 
MenuItemsSource ,
;, -
} 	
private 
new 
void %
ListView_SelectionChanged 2
(2 3
object3 9
sender: @
,@ A%
SelectionChangedEventArgsB [
e\ ]
)] ^
{ 	
base 
. %
ListView_SelectionChanged *
(* +
sender+ 1
,1 2
e3 4
)4 5
;5 6
var 
selectedItem 
= 
Model $
.$ %
SelectedItem% 1
as2 4
MainMenuItemVm5 C
;C D
if 
( 
selectedItem 
== 
null  $
)$ %
	MenuFrame& /
./ 0
Navigate0 8
(8 9
typeof9 ?
(? @
WelcomePage@ K
)K L
)L M
;M N
else 
selectedItem 
. 
Destination )
.) *
Navigate* 2
(2 3
selectedItem3 ?
.? @
PageType@ H
,H I
selectedItemJ V
.V W
	ParameterW `
)` a
;a b
} 	
	protected 
override 
void 
OnNavigatedTo  -
(- .
NavigationEventArgs. A
eB C
)C D
{   	
base!! 
.!! 
OnNavigatedTo!! 
(!! 
e!!  
)!!  !
;!!! "
DataContext"" 
="" 
new"" 
MainVm"" $
(""$ %
Frame""% *
,""* +
	MenuFrame"", 5
)""5 6
;""6 7
}## 	
}$$ 
}%% ˚
SC:\Sources\Other\ModernKeePass\ModernKeePass\Views\MainPageFrames\AboutPage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
	AboutPage  )
{		 
public

 
	AboutPage

 
(

 
)

 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} Û
YC:\Sources\Other\ModernKeePass\ModernKeePass\Views\MainPageFrames\NewDatabasePage.xaml.cs
	namespace		 	
ModernKeePass		
 
.		 
Views		 
{

 
public 

sealed 
partial 
class 
NewDatabasePage  /
{ 
public 
NewVm 
Model 
=> 
( 
NewVm $
)$ %
DataContext% 0
;0 1
public 
NewDatabasePage 
( 
)  
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
async 
void 
ButtonBase_OnClick -
(- .
object. 4
sender5 ;
,; <
RoutedEventArgs= L
eM N
)N O
{ 	
var 

savePicker 
= 
new  
FileSavePicker! /
{ "
SuggestedStartLocation &
=' (
PickerLocationId) 9
.9 :
DocumentsLibrary: J
,J K
SuggestedFileName !
=" #
$str$ 2
} 
; 

savePicker 
. 
FileTypeChoices &
.& '
Add' *
(* +
$str+ A
,A B
newC F
ListG K
<K L
stringL R
>R S
{T U
$strV ]
}^ _
)_ `
;` a
var   
file   
=   
await   

savePicker   '
.  ' (
PickSaveFileAsync  ( 9
(  9 :
)  : ;
;  ; <
if!! 
(!! 
file!! 
==!! 
null!! 
)!! 
return!! $
;!!$ %
Model"" 
."" 
OpenFile"" 
("" 
file"" 
)""  
;""  !
}## 	
}$$ 
}%% ú
GC:\Sources\Other\ModernKeePass\ModernKeePass\Views\SettingsPage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
SettingsPage  ,
{ 
public 
new 

SettingsVm 
Model #
=>$ &
(' (

SettingsVm( 2
)2 3
DataContext3 >
;> ?
public 
SettingsPage 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
ListView 
= 
MenuListView #
;# $
ListViewSource 
= 
MenuItemsSource ,
;, -
} 	
private 
new 
void %
ListView_SelectionChanged 2
(2 3
object3 9
sender: @
,@ A%
SelectionChangedEventArgsB [
e\ ]
)] ^
{ 	
base 
. %
ListView_SelectionChanged *
(* +
sender+ 1
,1 2
e3 4
)4 5
;5 6
var 
selectedItem 
= 
Model $
.$ %
SelectedItem% 1
as2 4
ListMenuItemVm5 C
;C D
	MenuFrame 
? 
. 
Navigate 
(  
selectedItem  ,
==- /
null0 4
?5 6
typeof7 =
(= >
SettingsWelcomePage> Q
)Q R
:S T
selectedItemU a
.a b
PageTypeb j
)j k
;k l
} 	
} 
} Å
UC:\Sources\Other\ModernKeePass\ModernKeePass\Views\MainPageFrames\WelcomePage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
WelcomePage  +
{		 
public

 
WelcomePage

 
(

 
)

 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} ‚
BC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\AboutVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
AboutVm 
{ 
private 
readonly 
Package  
_package! )
;) *
public		 
string		 
Name		 
=>		 
_package		 &
.		& '
DisplayName		' 2
;		2 3
public 
string 
Version 
{ 	
get 
{ 
var 
version 
= 
_package &
.& '
Id' )
.) *
Version* 1
;1 2
return 
$" 
{ 
version !
.! "
Major" '
}' (
.( )
{) *
version* 1
.1 2
Minor2 7
}7 8
"8 9
;9 :
} 
} 	
public 
AboutVm 
( 
) 
: 
this 
(  
Package  '
.' (
Current( /
)/ 0
{1 2
}3 4
public 
AboutVm 
( 
Package 
package &
)& '
{ 	
_package 
= 
package 
; 
} 	
} 
} àm
IC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\CompositeKeyVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
CompositeKeyVm 
:  %
NotifyPropertyChangedBase! :
{ 
public 
enum 
StatusTypes 
{ 	
Normal 
= 
$num 
, 
Error 
= 
$num 
, 
Warning 
= 
$num 
, 
Success 
= 
$num 
} 	
public 
IDatabaseService 
Database  (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
bool 
HasPassword 
{ 	
get 
{ 
return 
_hasPassword %
;% &
}' (
set 
{ 
SetProperty 
( 
ref 
_hasPassword  ,
,, -
value. 3
)3 4
;4 5
OnPropertyChanged   !
(  ! "
$str  " +
)  + ,
;  , -
}!! 
}"" 	
public$$ 
bool$$ 

HasKeyFile$$ 
{%% 	
get&& 
{&& 
return&& 
_hasKeyFile&& $
;&&$ %
}&&& '
set'' 
{(( 
SetProperty)) 
()) 
ref)) 
_hasKeyFile))  +
,))+ ,
value))- 2
)))2 3
;))3 4
OnPropertyChanged** !
(**! "
$str**" +
)**+ ,
;**, -
}++ 
},, 	
public.. 
bool.. 
HasUserAccount.. "
{// 	
get00 
{00 
return00 
_hasUserAccount00 (
;00( )
}00* +
set11 
{22 
SetProperty33 
(33 
ref33 
_hasUserAccount33  /
,33/ 0
value331 6
)336 7
;337 8
OnPropertyChanged44 !
(44! "
$str44" +
)44+ ,
;44, -
}55 
}66 	
public88 
bool88 
IsValid88 
=>88 
!88  

_isOpening88  *
&&88+ -
(88. /
HasPassword88/ :
||88; =

HasKeyFile88> H
&&88I K
KeyFile88L S
!=88T V
null88W [
||88\ ^
HasUserAccount88_ m
)88m n
;88n o
public:: 
string:: 
Status:: 
{;; 	
get<< 
{<< 
return<< 
_status<<  
;<<  !
}<<" #
set== 
{== 
SetProperty== 
(== 
ref== !
_status==" )
,==) *
value==+ 0
)==0 1
;==1 2
}==3 4
}>> 	
public@@ 
int@@ 

StatusType@@ 
{AA 	
getBB 
{BB 
returnBB 
(BB 
intBB 
)BB 
_statusTypeBB )
;BB) *
}BB+ ,
setCC 
{CC 
SetPropertyCC 
(CC 
refCC !
_statusTypeCC" -
,CC- .
(CC/ 0
StatusTypesCC0 ;
)CC; <
valueCC< A
)CCA B
;CCB C
}CCD E
}DD 	
publicFF 
stringFF 
PasswordFF 
{GG 	
getHH 
{HH 
returnHH 
	_passwordHH "
;HH" #
}HH$ %
setII 
{JJ 
	_passwordKK 
=KK 
valueKK !
;KK! "
OnPropertyChangedLL !
(LL! "
$strLL" ?
)LL? @
;LL@ A

StatusTypeMM 
=MM 
(MM 
intMM !
)MM! "
StatusTypesMM" -
.MM- .
NormalMM. 4
;MM4 5
StatusNN 
=NN 
stringNN 
.NN  
EmptyNN  %
;NN% &
}OO 
}PP 	
publicRR 
StorageFileRR 
KeyFileRR "
{SS 	
getTT 
{TT 
returnTT 
_keyFileTT !
;TT! "
}TT# $
setUU 
{VV 
_keyFileWW 
=WW 
valueWW  
;WW  !
KeyFileTextXX 
=XX 
valueXX #
?XX# $
.XX$ %
NameXX% )
;XX) *
OnPropertyChangedYY !
(YY! "
$strYY" +
)YY+ ,
;YY, -
}ZZ 
}[[ 	
public]] 
string]] 
KeyFileText]] !
{^^ 	
get__ 
{__ 
return__ 
_keyFileText__ %
;__% &
}__' (
set`` 
{`` 
SetProperty`` 
(`` 
ref`` !
_keyFileText``" .
,``. /
value``0 5
)``5 6
;``6 7
}``8 9
}aa 	
publiccc 
GroupVmcc 
	RootGroupcc  
{cc! "
getcc# &
;cc& '
setcc( +
;cc+ ,
}cc- .
publicee 
doubleee '
PasswordComplexityIndicatoree 1
=>ee2 4
QualityEstimationee5 F
.eeF G 
EstimatePasswordBitseeG [
(ee[ \
Passwordee\ d
?eed e
.eee f
ToCharArrayeef q
(eeq r
)eer s
)ees t
;eet u
privategg 
boolgg 
_hasPasswordgg !
;gg! "
privatehh 
boolhh 
_hasKeyFilehh  
;hh  !
privateii 
boolii 
_hasUserAccountii $
;ii$ %
privatejj 
booljj 

_isOpeningjj 
;jj  
privatekk 
stringkk 
	_passwordkk  
=kk! "
stringkk# )
.kk) *
Emptykk* /
;kk/ 0
privatell 
stringll 
_statusll 
;ll 
privatemm 
StatusTypesmm 
_statusTypemm '
;mm' (
privatenn 
StorageFilenn 
_keyFilenn $
;nn$ %
privateoo 
stringoo 
_keyFileTextoo #
;oo# $
privatepp 
readonlypp 
IResourceServicepp )
	_resourcepp* 3
;pp3 4
publicrr 
CompositeKeyVmrr 
(rr 
)rr 
:rr  !
thisrr" &
(rr& '
DatabaseServicerr' 6
.rr6 7
Instancerr7 ?
,rr? @
newrrA D
ResourcesServicerrE U
(rrU V
)rrV W
)rrW X
{rrY Z
}rr[ \
publictt 
CompositeKeyVmtt 
(tt 
IDatabaseServicett .
databasett/ 7
,tt7 8
IResourceServicett9 I
resourcettJ R
)ttR S
{uu 	
	_resourcevv 
=vv 
resourcevv  
;vv  !
_keyFileTextww 
=ww 
	_resourceww $
.ww$ %
GetResourceValueww% 5
(ww5 6
$strww6 R
)wwR S
;wwS T
Databasexx 
=xx 
databasexx 
;xx  
}yy 	
public{{ 
async{{ 
Task{{ 
<{{ 
bool{{ 
>{{ 
OpenDatabase{{  ,
({{, -
bool{{- 1
	createNew{{2 ;
){{; <
{|| 	
try}} 
{~~ 

_isOpening 
= 
true !
;! "
await
ÄÄ 
Database
ÄÄ 
.
ÄÄ 
Open
ÄÄ #
(
ÄÄ# $ 
CreateCompositeKey
ÄÄ$ 6
(
ÄÄ6 7
)
ÄÄ7 8
,
ÄÄ8 9
	createNew
ÄÄ: C
)
ÄÄC D
;
ÄÄD E
await
ÅÅ 
Task
ÅÅ 
.
ÅÅ 
Run
ÅÅ 
(
ÅÅ 
(
ÅÅ  
)
ÅÅ  !
=>
ÅÅ" $
	RootGroup
ÅÅ% .
=
ÅÅ/ 0
Database
ÅÅ1 9
.
ÅÅ9 :
	RootGroup
ÅÅ: C
)
ÅÅC D
;
ÅÅD E
return
ÇÇ 
true
ÇÇ 
;
ÇÇ 
}
ÉÉ 
catch
ÑÑ 
(
ÑÑ 
ArgumentException
ÑÑ $
)
ÑÑ$ %
{
ÖÖ 
var
ÜÜ 
errorMessage
ÜÜ  
=
ÜÜ! "
new
ÜÜ# &
StringBuilder
ÜÜ' 4
(
ÜÜ4 5
$"
ÜÜ5 7
{
ÜÜ7 8
	_resource
ÜÜ8 A
.
ÜÜA B
GetResourceValue
ÜÜB R
(
ÜÜR S
$str
ÜÜS j
)
ÜÜj k
}
ÜÜk l
\n
ÜÜl n
"
ÜÜn o
)
ÜÜo p
;
ÜÜp q
if
áá 
(
áá 
HasPassword
áá 
)
áá  
errorMessage
áá! -
.
áá- .

AppendLine
áá. 8
(
áá8 9
	_resource
áá9 B
.
ááB C
GetResourceValue
ááC S
(
ááS T
$str
ááT s
)
áás t
)
áát u
;
ááu v
if
àà 
(
àà 

HasKeyFile
àà 
)
àà 
errorMessage
àà  ,
.
àà, -

AppendLine
àà- 7
(
àà7 8
	_resource
àà8 A
.
ààA B
GetResourceValue
ààB R
(
ààR S
$str
ààS q
)
ààq r
)
ààr s
;
ààs t
if
ââ 
(
ââ 
HasUserAccount
ââ "
)
ââ" #
errorMessage
ââ$ 0
.
ââ0 1

AppendLine
ââ1 ;
(
ââ; <
	_resource
ââ< E
.
ââE F
GetResourceValue
ââF V
(
ââV W
$str
ââW u
)
ââu v
)
ââv w
;
ââw x
UpdateStatus
ää 
(
ää 
errorMessage
ää )
.
ää) *
ToString
ää* 2
(
ää2 3
)
ää3 4
,
ää4 5
StatusTypes
ää6 A
.
ääA B
Error
ääB G
)
ääG H
;
ääH I
}
ãã 
catch
åå 
(
åå 
	Exception
åå 
e
åå 
)
åå 
{
çç 
var
éé 
error
éé 
=
éé 
$"
éé 
{
éé 
	_resource
éé (
.
éé( )
GetResourceValue
éé) 9
(
éé9 :
$str
éé: Q
)
ééQ R
}
ééR S
{
ééS T
e
ééT U
.
ééU V
Message
ééV ]
}
éé] ^
"
éé^ _
;
éé_ `
UpdateStatus
èè 
(
èè 
error
èè "
,
èè" #
StatusTypes
èè$ /
.
èè/ 0
Error
èè0 5
)
èè5 6
;
èè6 7
}
êê 
finally
ëë 
{
íí 

_isOpening
ìì 
=
ìì 
false
ìì "
;
ìì" #
}
îî 
return
ïï 
false
ïï 
;
ïï 
}
ññ 	
public
òò 
void
òò 
	UpdateKey
òò 
(
òò 
)
òò 
{
ôô 	
Database
öö 
.
öö 
CompositeKey
öö !
=
öö" # 
CreateCompositeKey
öö$ 6
(
öö6 7
)
öö7 8
;
öö8 9
UpdateStatus
õõ 
(
õõ 
	_resource
õõ "
.
õõ" #
GetResourceValue
õõ# 3
(
õõ3 4
$str
õõ4 I
)
õõI J
,
õõJ K
StatusTypes
õõL W
.
õõW X
Success
õõX _
)
õõ_ `
;
õõ` a
}
úú 	
public
ûû 
void
ûû 
CreateKeyFile
ûû !
(
ûû! "
StorageFile
ûû" -
file
ûû. 2
)
ûû2 3
{
üü 	

KcpKeyFile
°° 
.
°° 
Create
°° 
(
°° 
file
°° "
,
°°" #
null
°°$ (
)
°°( )
;
°°) *
KeyFile
¢¢ 
=
¢¢ 
file
¢¢ 
;
¢¢ 
}
££ 	
private
•• 
void
•• 
UpdateStatus
•• !
(
••! "
string
••" (
text
••) -
,
••- .
StatusTypes
••/ :
type
••; ?
)
••? @
{
¶¶ 	
Status
ßß 
=
ßß 
text
ßß 
;
ßß 

StatusType
®® 
=
®® 
(
®® 
int
®® 
)
®® 
type
®® "
;
®®" #
}
©© 	
private
´´ 
CompositeKey
´´  
CreateCompositeKey
´´ /
(
´´/ 0
)
´´0 1
{
¨¨ 	
var
≠≠ 
compositeKey
≠≠ 
=
≠≠ 
new
≠≠ "
CompositeKey
≠≠# /
(
≠≠/ 0
)
≠≠0 1
;
≠≠1 2
if
ÆÆ 
(
ÆÆ 
HasPassword
ÆÆ 
)
ÆÆ 
compositeKey
ÆÆ )
.
ÆÆ) *

AddUserKey
ÆÆ* 4
(
ÆÆ4 5
new
ÆÆ5 8
KcpPassword
ÆÆ9 D
(
ÆÆD E
Password
ÆÆE M
)
ÆÆM N
)
ÆÆN O
;
ÆÆO P
if
ØØ 
(
ØØ 

HasKeyFile
ØØ 
&&
ØØ 
KeyFile
ØØ %
!=
ØØ& (
null
ØØ) -
)
ØØ- .
compositeKey
ØØ/ ;
.
ØØ; <

AddUserKey
ØØ< F
(
ØØF G
new
ØØG J

KcpKeyFile
ØØK U
(
ØØU V
IOConnectionInfo
ØØV f
.
ØØf g
FromFile
ØØg o
(
ØØo p
KeyFile
ØØp w
)
ØØw x
)
ØØx y
)
ØØy z
;
ØØz {
if
∞∞ 
(
∞∞ 
HasUserAccount
∞∞ 
)
∞∞ 
compositeKey
∞∞  ,
.
∞∞, -

AddUserKey
∞∞- 7
(
∞∞7 8
new
∞∞8 ;
KcpUserAccount
∞∞< J
(
∞∞J K
)
∞∞K L
)
∞∞L M
;
∞∞M N
return
±± 
compositeKey
±± 
;
±±  
}
≤≤ 	
}
≥≥ 
}¥¥ Ø
OC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\Items\ListMenuItemVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
ListMenuItemVm 
:  !%
NotifyPropertyChangedBase" ;
,; <

IIsEnabled= G
,G H
ISelectableModelI Y
{		 
private

 
bool

 
_isSelected

  
;

  !
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
Group 
{ 
get !
;! "
set# &
;& '
}( )
=* +
$str, /
;/ 0
public 
Type 
PageType 
{ 
get "
;" #
set$ '
;' (
}) *
public 
Symbol 

SymbolIcon  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
bool 
	IsEnabled 
{ 
get  #
;# $
set% (
;( )
}* +
=, -
true. 2
;2 3
public 
bool 

IsSelected 
{ 	
get 
{ 
return 
_isSelected $
;$ %
}& '
set 
{ 
SetProperty 
( 
ref !
_isSelected" -
,- .
value/ 4
)4 5
;5 6
}7 8
} 	
public 
override 
string 
ToString '
(' (
)( )
{ 	
return 
Title 
; 
} 	
} 
} â
OC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\Items\MainMenuItemVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
MainMenuItemVm 
:  
ListMenuItemVm! /
{ 
public 
object 
	Parameter 
{  !
get" %
;% &
set' *
;* +
}, -
public 
Frame 
Destination  
{! "
get# &
;& '
set( +
;+ ,
}- .
}		 
}

 ú
MC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\Items\RecentItemVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
RecentItemVm 
: %
NotifyPropertyChangedBase 8
,8 9
ISelectableModel: J
,J K
IRecentItemL W
{		 
private

 
bool

 
_isSelected

  
;

  !
public 
StorageFile 
DatabaseFile '
{( )
get* -
;- .
}/ 0
public 
string 
Token 
{ 
get !
;! "
}# $
public 
string 
Name 
{ 
get  
;  !
}" #
public 
string 
Path 
=> 
DatabaseFile *
?* +
.+ ,
Path, 0
;0 1
public 
bool 

IsSelected 
{ 	
get 
{ 
return 
_isSelected $
;$ %
}& '
set 
{ 
SetProperty 
( 
ref !
_isSelected" -
,- .
value/ 4
)4 5
;5 6
}7 8
} 	
public 
RecentItemVm 
( 
) 
{ 
}  
public 
RecentItemVm 
( 
string "
token# (
,( )
string* 0
metadata1 9
,9 :
IStorageItem; G
fileH L
)L M
{ 	
Token 
= 
token 
; 
Name 
= 
metadata 
; 
DatabaseFile 
= 
file 
as  "
StorageFile# .
;. /
} 	
public 
void 
OpenDatabaseFile $
($ %
)% &
{   	
OpenDatabaseFile!! 
(!! 
DatabaseService!! ,
.!!, -
Instance!!- 5
)!!5 6
;!!6 7
}"" 	
public$$ 
void$$ 
OpenDatabaseFile$$ $
($$$ %
IDatabaseService$$% 5
database$$6 >
)$$> ?
{%% 	
database&& 
.&& 
DatabaseFile&& !
=&&" #
DatabaseFile&&$ 0
;&&0 1
}'' 	
public)) 
void)) 
UpdateAccessTime)) $
())$ %
)))% &
{** 	
UpdateAccessTime++ 
(++ 
RecentService++ *
.++* +
Instance+++ 3
)++3 4
;++4 5
},, 	
public.. 
async.. 
void.. 
UpdateAccessTime.. *
(..* +
IRecentService..+ 9
recent..: @
)..@ A
{// 	
await00 
recent00 
.00 
GetFileAsync00 %
(00% &
Token00& +
)00+ ,
;00, -
}11 	
}22 
}33 •;
JC:\Sources\Other\ModernKeePass\ModernKeePass\Views\EntryDetailPage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
EntryDetailPage  /
{ 
public 
EntryVm 
Model 
=> 
(  !
EntryVm! (
)( )
DataContext* 5
;5 6
public 
NavigationHelper 
NavigationHelper  0
{1 2
get3 6
;6 7
}8 9
public 
EntryDetailPage 
( 
)  
{ 	
InitializeComponent 
(  
)  !
;! "
NavigationHelper 
= 
new "
NavigationHelper# 3
(3 4
this4 8
)8 9
;9 :
NavigationHelper 
. 
	LoadState &
+=' )&
navigationHelper_LoadState* D
;D E
}   	
private-- 
void-- &
navigationHelper_LoadState-- /
(--/ 0
object--0 6
sender--7 =
,--= >
LoadStateEventArgs--? Q
e--R S
)--S T
{--U V
}--V W
	protected:: 
override:: 
void:: 
OnNavigatedTo::  -
(::- .
NavigationEventArgs::. A
e::B C
)::C D
{;; 	
NavigationHelper<< 
.<< 
OnNavigatedTo<< *
(<<* +
e<<+ ,
)<<, -
;<<- .
if== 
(== 
!== 
(== 
e== 
.== 
	Parameter== 
is==  
EntryVm==! (
)==( )
)==) *
return==+ 1
;==1 2
DataContext>> 
=>> 
(>> 
EntryVm>> "
)>>" #
e>># $
.>>$ %
	Parameter>>% .
;>>. /
}?? 	
	protectedAA 
overrideAA 
voidAA 
OnNavigatedFromAA  /
(AA/ 0
NavigationEventArgsAA0 C
eAAD E
)AAE F
{BB 	
NavigationHelperCC 
.CC 
OnNavigatedFromCC ,
(CC, -
eCC- .
)CC. /
;CC/ 0
}DD 	
privateHH 
voidHH 
DeleteButton_ClickHH '
(HH' (
objectHH( .
senderHH/ 5
,HH5 6
RoutedEventArgsHH7 F
eHHG H
)HHH I
{II 	
varJJ 
resourceJJ 
=JJ 
newJJ 
ResourcesServiceJJ /
(JJ/ 0
)JJ0 1
;JJ1 2
varKK 
messageKK 
=KK 
ModelKK 
.KK  
IsRecycleOnDeleteKK  1
?LL 
resourceLL 
.LL 
GetResourceValueLL +
(LL+ ,
$strLL, H
)LLH I
:MM 
resourceMM 
.MM 
GetResourceValueMM +
(MM+ ,
$strMM, G
)MMG H
;MMH I
varNN 
textNN 
=NN 
ModelNN 
.NN 
IsRecycleOnDeleteNN .
?NN/ 0
resourceNN1 9
.NN9 :
GetResourceValueNN: J
(NNJ K
$strNNK Z
)NNZ [
:NN\ ]
resourceNN^ f
.NNf g
GetResourceValueNNg w
(NNw x
$str	NNx Ü
)
NNÜ á
;
NNá à
MessageDialogHelperOO 
.OO  
ShowActionDialogOO  0
(OO0 1
resourceOO1 9
.OO9 :
GetResourceValueOO: J
(OOJ K
$strOOK ^
)OO^ _
,OO_ `
messageOOa h
,OOh i
resourcePP 
.PP 
GetResourceValuePP )
(PP) *
$strPP* D
)PPD E
,PPE F
resourceQQ 
.QQ 
GetResourceValueQQ )
(QQ) *
$strQQ* D
)QQD E
,QQE F
aQQG H
=>QQI K
{RR #
ToastNotificationHelperSS '
.SS' (
ShowMovedToastSS( 6
(SS6 7
ModelSS7 <
,SS< =
resourceSS> F
.SSF G
GetResourceValueSSG W
(SSW X
$strSSX h
)SSh i
,SSi j
textSSk o
)SSo p
;SSp q
ModelTT 
.TT 
MarkForDeleteTT #
(TT# $
resourceTT$ ,
.TT, -
GetResourceValueTT- =
(TT= >
$strTT> O
)TTO P
)TTP Q
;TTQ R
ifUU 
(UU 
FrameUU 
.UU 
	CanGoBackUU #
)UU# $
FrameUU% *
.UU* +
GoBackUU+ 1
(UU1 2
)UU2 3
;UU3 4
}VV 
,VV 
nullVV 
)VV 
;VV 
}WW 	
privateYY 
voidYY 
RestoreButton_ClickYY (
(YY( )
objectYY) /
senderYY0 6
,YY6 7
RoutedEventArgsYY8 G
eYYH I
)YYI J
{ZZ 	
var[[ 
resource[[ 
=[[ 
new[[ 
ResourcesService[[ /
([[/ 0
)[[0 1
;[[1 2#
ToastNotificationHelper\\ #
.\\# $
ShowMovedToast\\$ 2
(\\2 3
Model\\3 8
,\\8 9
resource\\: B
.\\B C
GetResourceValue\\C S
(\\S T
$str\\T i
)\\i j
,\\j k
resource\\l t
.\\t u
GetResourceValue	\\u Ö
(
\\Ö Ü
$str
\\Ü ï
)
\\ï ñ
)
\\ñ ó
;
\\ó ò
if]] 
(]] 
Frame]] 
.]] 
	CanGoBack]] 
)]]  
Frame]]! &
.]]& '
GoBack]]' -
(]]- .
)]]. /
;]]/ 0
}^^ 	
private`` 
void`` )
EntryDetailPage_OnSizeChanged`` 2
(``2 3
object``3 9
sender``: @
,``@ A 
SizeChangedEventArgs``B V
e``W X
)``X Y
{aa 	
VisualStateManagerbb 
.bb 
	GoToStatebb (
(bb( )
thisbb) -
,bb- .
ebb/ 0
.bb0 1
NewSizebb1 8
.bb8 9
Widthbb9 >
<bb? @
$numbbA D
?bbE F
$strbbG N
:bbO P
$strbbQ X
,bbX Y
truebbZ ^
)bb^ _
;bb_ `
}cc 	
privateee 
voidee 7
+HamburgerMenuUserControl_OnSelectionChangedee @
(ee@ A
objecteeA G
sendereeH N
,eeN O%
SelectionChangedEventArgseeP i
eeej k
)eek l
{ff 	
vargg 
listViewgg 
=gg 
sendergg !
asgg" $
ListViewgg% -
;gg- .
EntryVmhh 
entryhh 
;hh 
switchii 
(ii 
listViewii 
?ii 
.ii 
SelectedIndexii +
)ii+ ,
{jj 
casekk 
-kk 
$numkk 
:kk 
returnll 
;ll 
defaultmm 
:mm 
entrynn 
=nn 
listViewnn $
?nn$ %
.nn% &
SelectedItemnn& 2
asnn3 5
EntryVmnn6 =
;nn= >
breakoo 
;oo 
}pp 

StackPanelrr 
.rr 
DataContextrr "
=rr# $
entryrr% *
;rr* +
}ss 	
}tt 
}uu är
JC:\Sources\Other\ModernKeePass\ModernKeePass\Views\GroupDetailPage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
GroupDetailPage  /
{ 
public 
NavigationHelper 
NavigationHelper  0
{1 2
get3 6
;6 7
}8 9
public 
GroupVm 
Model 
=> 
(  !
GroupVm! (
)( )
DataContext) 4
;4 5
public 
GroupDetailPage 
( 
)  
{ 	
InitializeComponent   
(    
)    !
;  ! "
NavigationHelper!! 
=!! 
new!! "
NavigationHelper!!# 3
(!!3 4
this!!4 8
)!!8 9
;!!9 :
NavigationHelper"" 
."" 
	LoadState"" &
+=""' )&
navigationHelper_LoadState""* D
;""D E
}## 	
private00 
void00 &
navigationHelper_LoadState00 /
(00/ 0
object000 6
sender007 =
,00= >
LoadStateEventArgs00? Q
e00R S
)00S T
{00U V
}00V W
	protected== 
override== 
void== 
OnNavigatedTo==  -
(==- .
NavigationEventArgs==. A
e==B C
)==C D
{>> 	
NavigationHelper?? 
.?? 
OnNavigatedTo?? *
(??* +
e??+ ,
)??, -
;??- .
ifAA 
(AA 
eAA 
.AA 
	ParameterAA 
isAA 
PasswordEventArgsAA 0
)AA0 1
DataContextBB 
=BB 
(BB 
(BB  
PasswordEventArgsBB  1
)BB1 2
eBB3 4
.BB4 5
	ParameterBB5 >
)BB> ?
.BB? @
	RootGroupBB@ I
;BBI J
elseCC 
ifCC 
(CC 
eCC 
.CC 
	ParameterCC  
isCC! #
GroupVmCC$ +
)CC+ ,
DataContextDD 
=DD 
(DD 
GroupVmDD &
)DD& '
eDD( )
.DD) *
	ParameterDD* 3
;DD3 4
}EE 	
	protectedGG 
overrideGG 
voidGG 
OnNavigatedFromGG  /
(GG/ 0
NavigationEventArgsGG0 C
eGGD E
)GGE F
{HH 	
NavigationHelperII 
.II 
OnNavigatedFromII ,
(II, -
eII- .
)II. /
;II/ 0
}JJ 	
privatePP 
voidPP #
groups_SelectionChangedPP ,
(PP, -
objectPP- 3
senderPP4 :
,PP: ;%
SelectionChangedEventArgsPP< U
ePPV W
)PPW X
{QQ 	
varRR 
listViewRR 
=RR 
senderRR !
asRR" $
ListViewRR% -
;RR- .
GroupVmSS 
groupSS 
;SS 
switchTT 
(TT 
listViewTT 
?TT 
.TT 
SelectedIndexTT +
)TT+ ,
{UU 
caseVV 
-VV 
$numVV 
:VV 
returnWW 
;WW 
defaultXX 
:XX 
groupYY 
=YY 
listViewYY $
?YY$ %
.YY% &
SelectedItemYY& 2
asYY3 5
GroupVmYY6 =
;YY= >
breakZZ 
;ZZ 
}[[ 
Frame\\ 
.\\ 
Navigate\\ 
(\\ 
typeof\\ !
(\\! "
GroupDetailPage\\" 1
)\\1 2
,\\2 3
group\\4 9
)\\9 :
;\\: ;
}]] 	
private__ 
void__ $
entries_SelectionChanged__ -
(__- .
object__. 4
sender__5 ;
,__; <%
SelectionChangedEventArgs__= V
e__W X
)__X Y
{`` 	
EntryVmaa 
entryaa 
;aa 
switchbb 
(bb 
GridViewbb 
.bb 
SelectedIndexbb *
)bb* +
{cc 
casedd 
-dd 
$numdd 
:dd 
returnee 
;ee 
defaultff 
:ff 
entrygg 
=gg 
GridViewgg $
.gg$ %
SelectedItemgg% 1
asgg2 4
EntryVmgg5 <
;gg< =
breakhh 
;hh 
}ii 
Framejj 
.jj 
Navigatejj 
(jj 
typeofjj !
(jj! "
EntryDetailPagejj" 1
)jj1 2
,jj2 3
entryjj4 9
)jj9 :
;jj: ;
}kk 	
privatemm 
voidmm 
DeleteButton_Clickmm '
(mm' (
objectmm( .
sendermm/ 5
,mm5 6
RoutedEventArgsmm7 F
emmG H
)mmH I
{nn 	
varoo 
resourceoo 
=oo 
newoo 
ResourcesServiceoo /
(oo/ 0
)oo0 1
;oo1 2
varpp 
messagepp 
=pp 
Modelpp 
.pp  
IsRecycleOnDeletepp  1
?qq 
resourceqq 
.qq 
GetResourceValueqq +
(qq+ ,
$strqq, H
)qqH I
:rr 
resourcerr 
.rr 
GetResourceValuerr +
(rr+ ,
$strrr, G
)rrG H
;rrH I
varss 
textss 
=ss 
Modelss 
.ss 
IsRecycleOnDeletess .
?ss/ 0
resourcess1 9
.ss9 :
GetResourceValuess: J
(ssJ K
$strssK Z
)ssZ [
:ss\ ]
resourcess^ f
.ssf g
GetResourceValuessg w
(ssw x
$str	ssx Ü
)
ssÜ á
;
ssá à
MessageDialogHelpertt 
.tt  
ShowActionDialogtt  0
(tt0 1
resourcett1 9
.tt9 :
GetResourceValuett: J
(ttJ K
$strttK ^
)tt^ _
,tt_ `
messagetta h
,tth i
resourceuu 
.uu 
GetResourceValueuu )
(uu) *
$struu* D
)uuD E
,uuE F
resourcevv 
.vv 
GetResourceValuevv )
(vv) *
$strvv* D
)vvD E
,vvE F
avvG H
=>vvI K
{ww #
ToastNotificationHelperxx +
.xx+ ,
ShowMovedToastxx, :
(xx: ;
Modelxx; @
,xx@ A
resourcexxB J
.xxJ K
GetResourceValuexxK [
(xx[ \
$strxx\ l
)xxl m
,xxm n
textxxo s
)xxs t
;xxt u
Modelyy 
.yy 
MarkForDeleteyy '
(yy' (
resourceyy( 0
.yy0 1
GetResourceValueyy1 A
(yyA B
$stryyB S
)yyS T
)yyT U
;yyU V
ifzz 
(zz 
Framezz 
.zz 
	CanGoBackzz '
)zz' (
Framezz) .
.zz. /
GoBackzz/ 5
(zz5 6
)zz6 7
;zz7 8
}{{ 
,{{ 
null{{ 
){{ 
;{{ 
}|| 	
private~~ 
void~~ 
RestoreButton_Click~~ (
(~~( )
object~~) /
sender~~0 6
,~~6 7
RoutedEventArgs~~8 G
e~~H I
)~~I J
{ 	
var
ÄÄ 
resource
ÄÄ 
=
ÄÄ 
new
ÄÄ 
ResourcesService
ÄÄ /
(
ÄÄ/ 0
)
ÄÄ0 1
;
ÄÄ1 2%
ToastNotificationHelper
ÅÅ #
.
ÅÅ# $
ShowMovedToast
ÅÅ$ 2
(
ÅÅ2 3
Model
ÅÅ3 8
,
ÅÅ8 9
resource
ÅÅ: B
.
ÅÅB C
GetResourceValue
ÅÅC S
(
ÅÅS T
$str
ÅÅT i
)
ÅÅi j
,
ÅÅj k
resource
ÇÇ 
.
ÇÇ 
GetResourceValue
ÇÇ )
(
ÇÇ) *
$str
ÇÇ* 9
)
ÇÇ9 :
)
ÇÇ: ;
;
ÇÇ; <
if
ÉÉ 
(
ÉÉ 
Frame
ÉÉ 
.
ÉÉ 
	CanGoBack
ÉÉ 
)
ÉÉ  
Frame
ÉÉ! &
.
ÉÉ& '
GoBack
ÉÉ' -
(
ÉÉ- .
)
ÉÉ. /
;
ÉÉ/ 0
}
ÑÑ 	
private
ÜÜ 
void
ÜÜ ,
SemanticZoom_ViewChangeStarted
ÜÜ 3
(
ÜÜ3 4
object
ÜÜ4 :
sender
ÜÜ; A
,
ÜÜA B.
 SemanticZoomViewChangedEventArgs
ÜÜC c
e
ÜÜd e
)
ÜÜe f
{
áá 	
if
ââ 
(
ââ 
e
ââ 
.
ââ "
IsSourceZoomedInView
ââ &
==
ââ' )
false
ââ* /
)
ââ/ 0
{
ää 
e
ãã 
.
ãã 
DestinationItem
ãã !
.
ãã! "
Item
ãã" &
=
ãã' (
e
ãã) *
.
ãã* +

SourceItem
ãã+ 5
.
ãã5 6
Item
ãã6 :
;
ãã: ;
}
åå 
}
çç 	
private
éé 
void
éé %
CreateEntry_ButtonClick
éé ,
(
éé, -
object
éé- 3
sender
éé4 :
,
éé: ;
RoutedEventArgs
éé< K
e
ééL M
)
ééM N
{
èè 	
Frame
êê 
.
êê 
Navigate
êê 
(
êê 
typeof
êê !
(
êê! "
EntryDetailPage
êê" 1
)
êê1 2
,
êê2 3
Model
êê4 9
.
êê9 :
AddNewEntry
êê: E
(
êêE F
)
êêF G
)
êêG H
;
êêH I
}
ëë 	
private
íí 
void
íí %
CreateGroup_ButtonClick
íí ,
(
íí, -
object
íí- 3
sender
íí4 :
,
íí: ;
RoutedEventArgs
íí< K
e
ííL M
)
ííM N
{
ìì 	
Frame
îî 
.
îî 
Navigate
îî 
(
îî 
typeof
îî !
(
îî! "
GroupDetailPage
îî" 1
)
îî1 2
,
îî2 3
Model
îî4 9
.
îî9 :
AddNewGroup
îî: E
(
îîE F
)
îîF G
)
îîG H
;
îîH I
}
ïï 	
private
óó 
void
óó (
GridView_DragItemsStarting
óó /
(
óó/ 0
object
óó0 6
sender
óó7 =
,
óó= >(
DragItemsStartingEventArgs
óó? Y
e
óóZ [
)
óó[ \
{
òò 	
e
ôô 
.
ôô 
Cancel
ôô 
=
ôô 
!
ôô 
Model
ôô 
.
ôô 

IsEditMode
ôô (
;
ôô( )
e
öö 
.
öö 
Data
öö 
.
öö  
RequestedOperation
öö %
=
öö& '"
DataPackageOperation
öö( <
.
öö< =
Move
öö= A
;
ööA B
}
õõ 	
private
ùù 
void
ùù .
 SearchBox_OnSuggestionsRequested
ùù 5
(
ùù5 6
	SearchBox
ùù6 ?
sender
ùù@ F
,
ùùF G4
&SearchBoxSuggestionsRequestedEventArgs
ùùH n
args
ùùo s
)
ùùs t
{
ûû 	
var
üü 
imageUri
üü 
=
üü )
RandomAccessStreamReference
üü 6
.
üü6 7
CreateFromUri
üü7 D
(
üüD E
new
üüE H
Uri
üüI L
(
üüL M
$strüüM Ñ
)üüÑ Ö
)üüÖ Ü
;üüÜ á
var
†† 
results
†† 
=
†† 
Model
†† 
.
††  

SubEntries
††  *
.
††* +
Where
††+ 0
(
††0 1
e
††1 2
=>
††3 5
e
††6 7
.
††7 8
Name
††8 <
.
††< =
IndexOf
††= D
(
††D E
args
††E I
.
††I J
	QueryText
††J S
,
††S T
StringComparison
††U e
.
††e f
OrdinalIgnoreCase
††f w
)
††w x
>=
††y {
$num
††| }
)
††} ~
.
††~ 
Take†† É
(††É Ñ
$num††Ñ Ö
)††Ö Ü
;††Ü á
foreach
°° 
(
°° 
var
°° 
result
°° 
in
°°  "
results
°°# *
)
°°* +
{
¢¢ 
args
££ 
.
££ 
Request
££ 
.
££ (
SearchSuggestionCollection
££ 7
.
££7 8$
AppendResultSuggestion
££8 N
(
££N O
result
££O U
.
££U V
Name
££V Z
,
££Z [
result
££\ b
.
££b c
ParentGroup
££c n
.
££n o
Name
££o s
,
££s t
result
££u {
.
££{ |
Id
££| ~
,
££~ 
imageUri££Ä à
,££à â
string££ä ê
.££ê ë
Empty££ë ñ
)££ñ ó
;££ó ò
}
§§ 
}
•• 	
private
ßß 
void
ßß 0
"SearchBox_OnResultSuggestionChosen
ßß 7
(
ßß7 8
	SearchBox
ßß8 A
sender
ßßB H
,
ßßH I6
(SearchBoxResultSuggestionChosenEventArgs
ßßJ r
args
ßßs w
)
ßßw x
{
®® 	
var
©© 
entry
©© 
=
©© 
Model
©© 
.
©© 

SubEntries
©© (
.
©©( )
FirstOrDefault
©©) 7
(
©©7 8
e
©©8 9
=>
©©: <
e
©©= >
.
©©> ?
Id
©©? A
==
©©B D
args
©©E I
.
©©I J
Tag
©©J M
)
©©M N
;
©©N O
Frame
™™ 
.
™™ 
Navigate
™™ 
(
™™ 
typeof
™™ !
(
™™! "
EntryDetailPage
™™" 1
)
™™1 2
,
™™2 3
entry
™™4 9
)
™™9 :
;
™™: ;
}
´´ 	
private
≠≠ 
void
≠≠ +
GroupDetailPage_OnSizeChanged
≠≠ 2
(
≠≠2 3
object
≠≠3 9
sender
≠≠: @
,
≠≠@ A"
SizeChangedEventArgs
≠≠B V
e
≠≠W X
)
≠≠X Y
{
ÆÆ 	 
VisualStateManager
ØØ 
.
ØØ 
	GoToState
ØØ (
(
ØØ( )
this
ØØ) -
,
ØØ- .
e
ØØ/ 0
.
ØØ0 1
NewSize
ØØ1 8
.
ØØ8 9
Width
ØØ9 >
<
ØØ? @
$num
ØØA D
?
ØØE F
$str
ØØG N
:
ØØO P
$str
ØØQ X
,
ØØX Y
true
ØØZ ^
)
ØØ^ _
;
ØØ_ `
}
∞∞ 	
}
≥≥ 
}¥¥ ñ
ZC:\Sources\Other\ModernKeePass\ModernKeePass\Views\MainPageFrames\OpenDatabasePage.xaml.cs
	namespace

 	
ModernKeePass


 
.

 
Views

 
{ 
public 

sealed 
partial 
class 
OpenDatabasePage  0
{ 
private 
Frame 

_mainFrame  
;  !
public 
OpenVm 
Model 
=> 
(  
OpenVm  &
)& '
DataContext' 2
;2 3
public 
OpenDatabasePage 
(  
)  !
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
	protected 
override 
void 
OnNavigatedTo  -
(- .
NavigationEventArgs. A
eB C
)C D
{ 	
base 
. 
OnNavigatedTo 
( 
e  
)  !
;! "

_mainFrame 
= 
e 
. 
	Parameter $
as% '
Frame( -
;- .
} 	
private   
async   
void   
ButtonBase_OnClick   -
(  - .
object  . 4
sender  5 ;
,  ; <
RoutedEventArgs  = L
e  M N
)  N O
{!! 	
var"" 
picker"" 
="" 
new## 
FileOpenPicker## "
{$$ 
ViewMode%% 
=%% 
PickerViewMode%% -
.%%- .
List%%. 2
,%%2 3"
SuggestedStartLocation&& *
=&&+ ,
PickerLocationId&&- =
.&&= >
DocumentsLibrary&&> N
}'' 
;'' 
picker(( 
.(( 
FileTypeFilter(( !
.((! "
Add((" %
(((% &
$str((& -
)((- .
;((. /
var++ 
file++ 
=++ 
await++ 
picker++ #
.++# $
PickSingleFileAsync++$ 7
(++7 8
)++8 9
;++9 :
if,, 
(,, 
file,, 
==,, 
null,, 
),, 
return,, $
;,,$ %
Model-- 
.-- 
OpenFile-- 
(-- 
file-- 
)--  
;--  !
}.. 	
}// 
}00 ô
]C:\Sources\Other\ModernKeePass\ModernKeePass\Views\MainPageFrames\RecentDatabasesPage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
RecentDatabasesPage  3
{		 
public

 
RecentDatabasesPage

 "
(

" #
)

# $
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} ‚
ZC:\Sources\Other\ModernKeePass\ModernKeePass\Views\MainPageFrames\SaveDatabasePage.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
{ 
public 

sealed 
partial 
class 
SaveDatabasePage  0
{ 
private 
Frame 

_mainFrame  
;  !
public 
SaveVm 
Model 
=> 
(  
SaveVm  &
)& '
DataContext' 2
;2 3
public 
SaveDatabasePage 
(  
)  !
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
	protected 
override 
void 
OnNavigatedTo  -
(- .
NavigationEventArgs. A
eB C
)C D
{ 	
base 
. 
OnNavigatedTo 
( 
e  
)  !
;! "

_mainFrame 
= 
e 
. 
	Parameter $
as% '
Frame( -
;- .
} 	
private 
async 
void 
SaveButton_OnClick -
(- .
object. 4
sender5 ;
,; <
RoutedEventArgs= L
eM N
)N O
{   	
await!! 
Model!! 
.!! 
Save!! 
(!! 
)!! 
;!! 

_mainFrame"" 
."" 
Navigate"" 
(""  
typeof""  &
(""& '
MainPage""' /
)""/ 0
)""0 1
;""1 2
}## 	
private%% 
async%% 
void%%  
SaveAsButton_OnClick%% /
(%%/ 0
object%%0 6
sender%%7 =
,%%= >
RoutedEventArgs%%? N
e%%O P
)%%P Q
{&& 	
var'' 

savePicker'' 
='' 
new''  
FileSavePicker''! /
{(( "
SuggestedStartLocation)) &
=))' (
PickerLocationId))) 9
.))9 :
DocumentsLibrary)): J
,))J K
SuggestedFileName** !
=**" #
$str**$ 2
}++ 
;++ 

savePicker,, 
.,, 
FileTypeChoices,, &
.,,& '
Add,,' *
(,,* +
$str,,+ A
,,,A B
new,,C F
List,,G K
<,,K L
string,,L R
>,,R S
{,,T U
$str,,V ]
},,^ _
),,_ `
;,,` a
var.. 
file.. 
=.. 
await.. 

savePicker.. '
...' (
PickSaveFileAsync..( 9
(..9 :
)..: ;
;..; <
if// 
(// 
file// 
==// 
null// 
)// 
return// $
;//$ %
Model00 
.00 
Save00 
(00 
file00 
)00 
;00 

_mainFrame22 
.22 
Navigate22 
(22  
typeof22  &
(22& '
MainPage22' /
)22/ 0
)220 1
;221 2
}33 	
}44 
}55 Ô
GC:\Sources\Other\ModernKeePass\ModernKeePass\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str (
)( )
]) *
[ 
assembly 	
:	 

AssemblyDescription 
( 
$str b
)b c
]c d
[		 
assembly		 	
:			 
!
AssemblyConfiguration		  
(		  !
$str		! #
)		# $
]		$ %
[

 
assembly

 	
:

	 

AssemblyCompany

 
(

 
$str

 #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str *
)* +
]+ ,
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str /
)/ 0
]0 1
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyVersion 
( 
$str %
)% &
]& '
[ 
assembly 	
:	 

AssemblyFileVersion 
( 
$str )
)) *
]* +
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] ¿ƒ
BC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\EntryVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
EntryVm 
: "
INotifyPropertyChanged 1
,1 2
	IPwEntity3 <
,< =
ISelectableModel> N
{ 
public 
GroupVm 
ParentGroup "
{# $
get% (
;( )
private* 1
set2 5
;5 6
}7 8
public 
GroupVm 
PreviousGroup $
{% &
get' *
;* +
private, 3
set4 7
;7 8
}9 :
public 
System 
. 
Drawing 
. 
Color #
?# $
BackgroundColor% 4
=>5 7
_pwEntry8 @
?@ A
.A B
BackgroundColorB Q
;Q R
public 
System 
. 
Drawing 
. 
Color #
?# $
ForegroundColor% 4
=>5 7
_pwEntry8 @
?@ A
.A B
ForegroundColorB Q
;Q R
public 
bool #
IsRevealPasswordEnabled +
=>, .
!/ 0
string0 6
.6 7
IsNullOrEmpty7 D
(D E
PasswordE M
)M N
;N O
public 
bool 

HasExpired 
=> !
HasExpirationDate" 3
&&4 6
_pwEntry7 ?
.? @

ExpiryTime@ J
<K L
DateTimeM U
.U V
NowV Y
;Y Z
public 
double '
PasswordComplexityIndicator 1
=>2 4
QualityEstimation5 F
.F G 
EstimatePasswordBitsG [
([ \
Password\ d
?d e
.e f
ToCharArrayf q
(q r
)r s
)s t
;t u
public 
bool $
UpperCasePatternSelected ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
=; <
true= A
;A B
public 
bool $
LowerCasePatternSelected ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
=; <
true= A
;A B
public 
bool !
DigitsPatternSelected )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
=8 9
true: >
;> ?
public 
bool  
MinusPatternSelected (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
bool %
UnderscorePatternSelected -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
public 
bool  
SpacePatternSelected (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
bool "
SpecialPatternSelected *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
bool #
BracketsPatternSelected +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
string 
CustomChars !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public   
PwUuid   
IdUuid   
=>   
_pwEntry    (
?  ( )
.  ) *
Uuid  * .
;  . /
public!! 
string!! 
Id!! 
=>!! 
_pwEntry!! $
?!!$ %
.!!% &
Uuid!!& *
.!!* +
ToHexString!!+ 6
(!!6 7
)!!7 8
;!!8 9
public"" 
bool"" 
IsRecycleOnDelete"" %
=>""& (
	_database"") 2
.""2 3
RecycleBinEnabled""3 D
&&""E G
!""H I
ParentGroup""I T
.""T U

IsSelected""U _
;""_ `
public## 
IEnumerable## 
<## 
	IPwEntity## $
>##$ %

BreadCrumb##& 0
=>##1 3
new##4 7
List##8 <
<##< =
	IPwEntity##= F
>##F G
(##G H
ParentGroup##H S
.##S T

BreadCrumb##T ^
)##^ _
{##` a
ParentGroup##a l
}##l m
;##m n
public'' 
bool'' 

IsSelected'' 
{''  
get''! $
;''$ %
set''& )
;'') *
}''+ ,
=''- .
true''/ 3
;''3 4
public)) 
double)) 
PasswordLength)) $
{** 	
get++ 
{++ 
return++ 
_passwordLength++ (
;++( )
}++* +
set,, 
{-- 
_passwordLength.. 
=..  !
value.." '
;..' (!
NotifyPropertyChanged// %
(//% &
$str//& 6
)//6 7
;//7 8
}00 
}11 	
public33 
string33 
Name33 
{44 	
get55 
{55 
return55 
GetEntryValue55 &
(55& '
PwDefs55' -
.55- .

TitleField55. 8
)558 9
;559 :
}55; <
set66 
{66 
SetEntryValue66 
(66  
PwDefs66  &
.66& '

TitleField66' 1
,661 2
value663 8
)668 9
;669 :
}66; <
}77 	
public:: 
string:: 
UserName:: 
{;; 	
get<< 
{<< 
return<< 
GetEntryValue<< &
(<<& '
PwDefs<<' -
.<<- .
UserNameField<<. ;
)<<; <
;<<< =
}<<> ?
set== 
{== 
SetEntryValue== 
(==  
PwDefs==  &
.==& '
UserNameField==' 4
,==4 5
value==6 ;
)==; <
;==< =
}==> ?
}>> 	
public@@ 
string@@ 
Password@@ 
{AA 	
getBB 
{BB 
returnBB 
GetEntryValueBB &
(BB& '
PwDefsBB' -
.BB- .
PasswordFieldBB. ;
)BB; <
;BB< =
}BB> ?
setCC 
{DD 
SetEntryValueEE 
(EE 
PwDefsEE $
.EE$ %
PasswordFieldEE% 2
,EE2 3
valueEE4 9
)EE9 :
;EE: ;!
NotifyPropertyChangedFF %
(FF% &
$strFF& 0
)FF0 1
;FF1 2!
NotifyPropertyChangedGG %
(GG% &
$strGG& C
)GGC D
;GGD E
}HH 
}II 	
publicKK 
stringKK 
UrlKK 
{LL 	
getMM 
{MM 
returnMM 
GetEntryValueMM &
(MM& '
PwDefsMM' -
.MM- .
UrlFieldMM. 6
)MM6 7
;MM7 8
}MM9 :
setNN 
{NN 
SetEntryValueNN 
(NN  
PwDefsNN  &
.NN& '
UrlFieldNN' /
,NN/ 0
valueNN1 6
)NN6 7
;NN7 8
}NN9 :
}OO 	
publicQQ 
stringQQ 
NotesQQ 
{RR 	
getSS 
{SS 
returnSS 
GetEntryValueSS &
(SS& '
PwDefsSS' -
.SS- .

NotesFieldSS. 8
)SS8 9
;SS9 :
}SS; <
setTT 
{TT 
SetEntryValueTT 
(TT  
PwDefsTT  &
.TT& '

NotesFieldTT' 1
,TT1 2
valueTT3 8
)TT8 9
;TT9 :
}TT; <
}UU 	
publicWW 
intWW 
IconIdWW 
{XX 	
getYY 
{ZZ 
if[[ 
([[ 
_pwEntry[[ 
?[[ 
.[[ 
IconId[[ $
!=[[% '
null[[( ,
)[[, -
return[[. 4
([[5 6
int[[6 9
)[[9 :
_pwEntry[[; C
?[[C D
.[[D E
IconId[[E K
;[[K L
return\\ 
-\\ 
$num\\ 
;\\ 
}]] 
}^^ 	
public`` 
DateTimeOffset`` 

ExpiryDate`` (
{aa 	
getbb 
{bb 
returnbb 
newbb 
DateTimeOffsetbb +
(bb+ ,
_pwEntrybb, 4
.bb4 5

ExpiryTimebb5 ?
.bb? @
Datebb@ D
)bbD E
;bbE F
}bbG H
setcc 
{cc 
ifcc 
(cc 
HasExpirationDatecc '
)cc' (
_pwEntrycc) 1
.cc1 2

ExpiryTimecc2 <
=cc= >
valuecc? D
.ccD E
DateTimeccE M
;ccM N
}ccO P
}dd 	
publicff 
TimeSpanff 

ExpiryTimeff "
{gg 	
gethh 
{hh 
returnhh 
_pwEntryhh !
.hh! "

ExpiryTimehh" ,
.hh, -
	TimeOfDayhh- 6
;hh6 7
}hh8 9
setii 
{ii 
ifii 
(ii 
HasExpirationDateii '
)ii' (
_pwEntryii) 1
.ii1 2

ExpiryTimeii2 <
=ii= >
_pwEntryii? G
.iiG H

ExpiryTimeiiH R
.iiR S
DateiiS W
.iiW X
AddiiX [
(ii[ \
valueii\ a
)iia b
;iib c
}iid e
}jj 	
publicll 
boolll 

IsEditModell 
{mm 	
getnn 
{nn 
returnnn 

IsSelectednn #
&&nn$ &
_isEditModenn' 2
;nn2 3
}nn4 5
setoo 
{pp 
_isEditModeqq 
=qq 
valueqq #
;qq# $!
NotifyPropertyChangedrr %
(rr% &
$strrr& 2
)rr2 3
;rr3 4
}ss 
}tt 	
publicvv 
boolvv 
	IsVisiblevv 
{ww 	
getxx 
{xx 
returnxx 

_isVisiblexx #
;xx# $
}xx% &
setyy 
{zz 

_isVisible{{ 
={{ 
value{{ "
;{{" #!
NotifyPropertyChanged|| %
(||% &
$str||& 1
)||1 2
;||2 3
}}} 
}~~ 	
public
ÄÄ 
bool
ÄÄ 
IsRevealPassword
ÄÄ $
{
ÅÅ 	
get
ÇÇ 
{
ÇÇ 
return
ÇÇ 
_isRevealPassword
ÇÇ *
;
ÇÇ* +
}
ÇÇ, -
set
ÉÉ 
{
ÑÑ 
_isRevealPassword
ÖÖ !
=
ÖÖ" #
value
ÖÖ$ )
;
ÖÖ) *#
NotifyPropertyChanged
ÜÜ %
(
ÜÜ% &
$str
ÜÜ& 8
)
ÜÜ8 9
;
ÜÜ9 :
}
áá 
}
àà 	
public
ââ 
bool
ââ 
HasExpirationDate
ââ %
{
ää 	
get
ãã 
{
ãã 
return
ãã 
_pwEntry
ãã !
.
ãã! "
Expires
ãã" )
;
ãã) *
}
ãã+ ,
set
åå 
{
çç 
_pwEntry
éé 
.
éé 
Expires
éé  
=
éé! "
value
éé# (
;
éé( )#
NotifyPropertyChanged
èè %
(
èè% &
$str
èè& 9
)
èè9 :
;
èè: ;
}
êê 
}
ëë 	
public
ìì 
IEnumerable
ìì 
<
ìì 
	IPwEntity
ìì $
>
ìì$ %
History
ìì& -
{
îî 	
get
ïï 
{
ññ 
var
óó 
history
óó 
=
óó 
new
óó !
List
óó" &
<
óó& '
EntryVm
óó' .
>
óó. /
{
óó0 1
this
óó1 5
}
óó5 6
;
óó6 7
foreach
òò 
(
òò 
var
òò 
historyEntry
òò )
in
òò* ,
_pwEntry
òò- 5
.
òò5 6
History
òò6 =
)
òò= >
{
ôô 
history
öö 
.
öö 
Add
öö 
(
öö  
new
öö  #
EntryVm
öö$ +
(
öö+ ,
historyEntry
öö, 8
,
öö8 9
ParentGroup
öö: E
)
ööE F
{
ööG H

IsSelected
ööH R
=
ööS T
false
ööU Z
}
ööZ [
)
öö[ \
;
öö\ ]
}
õõ 
return
ùù 
history
ùù 
;
ùù 
}
ûû 
}
üü 	
public
°° 
event
°° )
PropertyChangedEventHandler
°° 0
PropertyChanged
°°1 @
;
°°@ A
private
££ 
readonly
££ 
PwEntry
££  
_pwEntry
££! )
;
££) *
private
§§ 
readonly
§§ 
IDatabaseService
§§ )
	_database
§§* 3
;
§§3 4
private
•• 
readonly
•• 
IResourceService
•• )
	_resource
••* 3
;
••3 4
private
¶¶ 
bool
¶¶ 
_isEditMode
¶¶  
;
¶¶  !
private
ßß 
bool
ßß 
_isRevealPassword
ßß &
;
ßß& '
private
®® 
double
®® 
_passwordLength
®® &
=
®®' (
$num
®®) +
;
®®+ ,
private
©© 
bool
©© 

_isVisible
©© 
=
©©  !
true
©©" &
;
©©& '
private
´´ 
void
´´ #
NotifyPropertyChanged
´´ *
(
´´* +
string
´´+ 1
propertyName
´´2 >
)
´´> ?
{
¨¨ 	
PropertyChanged
≠≠ 
?
≠≠ 
.
≠≠ 
Invoke
≠≠ #
(
≠≠# $
this
≠≠$ (
,
≠≠( )
new
≠≠* -&
PropertyChangedEventArgs
≠≠. F
(
≠≠F G
propertyName
≠≠G S
)
≠≠S T
)
≠≠T U
;
≠≠U V
}
ÆÆ 	
public
∞∞ 
EntryVm
∞∞ 
(
∞∞ 
)
∞∞ 
{
∞∞ 
}
∞∞ 
internal
≤≤ 
EntryVm
≤≤ 
(
≤≤ 
PwEntry
≤≤  
entry
≤≤! &
,
≤≤& '
GroupVm
≤≤( /
parent
≤≤0 6
)
≤≤6 7
:
≤≤8 9
this
≤≤: >
(
≤≤> ?
entry
≤≤? D
,
≤≤D E
parent
≤≤F L
,
≤≤L M
DatabaseService
≤≤N ]
.
≤≤] ^
Instance
≤≤^ f
,
≤≤f g
new
≤≤h k
ResourcesService
≤≤l |
(
≤≤| }
)
≤≤} ~
)
≤≤~ 
{≤≤Ä Å
}≤≤Ç É
public
¥¥ 
EntryVm
¥¥ 
(
¥¥ 
PwEntry
¥¥ 
entry
¥¥ $
,
¥¥$ %
GroupVm
¥¥& -
parent
¥¥. 4
,
¥¥4 5
IDatabaseService
¥¥6 F
database
¥¥G O
,
¥¥O P
IResourceService
¥¥Q a
resource
¥¥b j
)
¥¥j k
{
µµ 	
	_database
∂∂ 
=
∂∂ 
database
∂∂  
;
∂∂  !
	_resource
∑∑ 
=
∑∑ 
resource
∑∑  
;
∑∑  !
_pwEntry
∏∏ 
=
∏∏ 
entry
∏∏ 
;
∏∏ 
ParentGroup
ππ 
=
ππ 
parent
ππ  
;
ππ  !
}
∫∫ 	
public
ºº 
void
ºº 
GeneratePassword
ºº $
(
ºº$ %
)
ºº% &
{
ΩΩ 	
var
ææ 
	pwProfile
ææ 
=
ææ 
new
ææ 
	PwProfile
ææ  )
{
øø 
GeneratorType
¿¿ 
=
¿¿ #
PasswordGeneratorType
¿¿  5
.
¿¿5 6
CharSet
¿¿6 =
,
¿¿= >
Length
¡¡ 
=
¡¡ 
(
¡¡ 
uint
¡¡ 
)
¡¡ 
PasswordLength
¡¡ -
,
¡¡- .
CharSet
¬¬ 
=
¬¬ 
new
¬¬ 
	PwCharSet
¬¬ '
(
¬¬' (
)
¬¬( )
}
√√ 
;
√√ 
if
≈≈ 
(
≈≈ &
UpperCasePatternSelected
≈≈ (
)
≈≈( )
	pwProfile
≈≈* 3
.
≈≈3 4
CharSet
≈≈4 ;
.
≈≈; <
Add
≈≈< ?
(
≈≈? @
	PwCharSet
≈≈@ I
.
≈≈I J
	UpperCase
≈≈J S
)
≈≈S T
;
≈≈T U
if
∆∆ 
(
∆∆ &
LowerCasePatternSelected
∆∆ (
)
∆∆( )
	pwProfile
∆∆* 3
.
∆∆3 4
CharSet
∆∆4 ;
.
∆∆; <
Add
∆∆< ?
(
∆∆? @
	PwCharSet
∆∆@ I
.
∆∆I J
	LowerCase
∆∆J S
)
∆∆S T
;
∆∆T U
if
«« 
(
«« #
DigitsPatternSelected
«« %
)
««% &
	pwProfile
««' 0
.
««0 1
CharSet
««1 8
.
««8 9
Add
««9 <
(
««< =
	PwCharSet
««= F
.
««F G
Digits
««G M
)
««M N
;
««N O
if
»» 
(
»» $
SpecialPatternSelected
»» &
)
»»& '
	pwProfile
»»( 1
.
»»1 2
CharSet
»»2 9
.
»»9 :
Add
»»: =
(
»»= >
	PwCharSet
»»> G
.
»»G H
SpecialChars
»»H T
)
»»T U
;
»»U V
if
…… 
(
…… "
MinusPatternSelected
…… $
)
……$ %
	pwProfile
……& /
.
……/ 0
CharSet
……0 7
.
……7 8
Add
……8 ;
(
……; <
$char
……< ?
)
……? @
;
……@ A
if
   
(
   '
UnderscorePatternSelected
   )
)
  ) *
	pwProfile
  + 4
.
  4 5
CharSet
  5 <
.
  < =
Add
  = @
(
  @ A
$char
  A D
)
  D E
;
  E F
if
ÀÀ 
(
ÀÀ "
SpacePatternSelected
ÀÀ $
)
ÀÀ$ %
	pwProfile
ÀÀ& /
.
ÀÀ/ 0
CharSet
ÀÀ0 7
.
ÀÀ7 8
Add
ÀÀ8 ;
(
ÀÀ; <
$char
ÀÀ< ?
)
ÀÀ? @
;
ÀÀ@ A
if
ÃÃ 
(
ÃÃ %
BracketsPatternSelected
ÃÃ '
)
ÃÃ' (
	pwProfile
ÃÃ) 2
.
ÃÃ2 3
CharSet
ÃÃ3 :
.
ÃÃ: ;
Add
ÃÃ; >
(
ÃÃ> ?
	PwCharSet
ÃÃ? H
.
ÃÃH I
Brackets
ÃÃI Q
)
ÃÃQ R
;
ÃÃR S
	pwProfile
ŒŒ 
.
ŒŒ 
CharSet
ŒŒ 
.
ŒŒ 
Add
ŒŒ !
(
ŒŒ! "
CustomChars
ŒŒ" -
)
ŒŒ- .
;
ŒŒ. /
ProtectedString
–– 
password
–– $
;
––$ %
PwGenerator
—— 
.
—— 
Generate
——  
(
——  !
out
——! $
password
——% -
,
——- .
	pwProfile
——/ 8
,
——8 9
null
——: >
,
——> ?
new
——@ C#
CustomPwGeneratorPool
——D Y
(
——Y Z
)
——Z [
)
——[ \
;
——\ ]
_pwEntry
”” 
?
”” 
.
”” 
Strings
”” 
.
”” 
Set
”” !
(
””! "
PwDefs
””" (
.
””( )
PasswordField
””) 6
,
””6 7
password
””8 @
)
””@ A
;
””A B#
NotifyPropertyChanged
‘‘ !
(
‘‘! "
$str
‘‘" ,
)
‘‘, -
;
‘‘- .#
NotifyPropertyChanged
’’ !
(
’’! "
$str
’’" ;
)
’’; <
;
’’< =#
NotifyPropertyChanged
÷÷ !
(
÷÷! "
$str
÷÷" ?
)
÷÷? @
;
÷÷@ A
}
◊◊ 	
private
ŸŸ 
string
ŸŸ 
GetEntryValue
ŸŸ $
(
ŸŸ$ %
string
ŸŸ% +
key
ŸŸ, /
)
ŸŸ/ 0
{
⁄⁄ 	
return
€€ 
_pwEntry
€€ 
?
€€ 
.
€€ 
Strings
€€ $
.
€€$ %
GetSafe
€€% ,
(
€€, -
key
€€- 0
)
€€0 1
.
€€1 2

ReadString
€€2 <
(
€€< =
)
€€= >
;
€€> ?
}
‹‹ 	
private
ﬁﬁ 
void
ﬁﬁ 
SetEntryValue
ﬁﬁ "
(
ﬁﬁ" #
string
ﬁﬁ# )
key
ﬁﬁ* -
,
ﬁﬁ- .
string
ﬁﬁ/ 5
newValue
ﬁﬁ6 >
)
ﬁﬁ> ?
{
ﬂﬂ 	
_pwEntry
‡‡ 
?
‡‡ 
.
‡‡ 
Strings
‡‡ 
.
‡‡ 
Set
‡‡ !
(
‡‡! "
key
‡‡" %
,
‡‡% &
new
‡‡' *
ProtectedString
‡‡+ :
(
‡‡: ;
true
‡‡; ?
,
‡‡? @
newValue
‡‡A I
)
‡‡I J
)
‡‡J K
;
‡‡K L
}
·· 	
public
„„ 
void
„„ 
MarkForDelete
„„ !
(
„„! "
string
„„" (
recycleBinTitle
„„) 8
)
„„8 9
{
‰‰ 	
if
ÂÂ 
(
ÂÂ 
	_database
ÂÂ 
.
ÂÂ 
RecycleBinEnabled
ÂÂ +
&&
ÂÂ, .
	_database
ÂÂ/ 8
.
ÂÂ8 9

RecycleBin
ÂÂ9 C
?
ÂÂC D
.
ÂÂD E
IdUuid
ÂÂE K
==
ÂÂL N
null
ÂÂO S
)
ÂÂS T
	_database
ÊÊ 
.
ÊÊ 
CreateRecycleBin
ÊÊ *
(
ÊÊ* +
recycleBinTitle
ÊÊ+ :
)
ÊÊ: ;
;
ÊÊ; <
Move
ÁÁ 
(
ÁÁ 
	_database
ÁÁ 
.
ÁÁ 
RecycleBinEnabled
ÁÁ ,
&&
ÁÁ- /
!
ÁÁ0 1
ParentGroup
ÁÁ1 <
.
ÁÁ< =

IsSelected
ÁÁ= G
?
ÁÁH I
	_database
ÁÁJ S
.
ÁÁS T

RecycleBin
ÁÁT ^
:
ÁÁ_ `
null
ÁÁa e
)
ÁÁe f
;
ÁÁf g
}
ËË 	
public
ÍÍ 
void
ÍÍ 

UndoDelete
ÍÍ 
(
ÍÍ 
)
ÍÍ  
{
ÎÎ 	
Move
ÏÏ 
(
ÏÏ 
PreviousGroup
ÏÏ 
)
ÏÏ 
;
ÏÏ  
}
ÌÌ 	
public
ÔÔ 
void
ÔÔ 
Move
ÔÔ 
(
ÔÔ 
GroupVm
ÔÔ  
destination
ÔÔ! ,
)
ÔÔ, -
{
 	
PreviousGroup
ÒÒ 
=
ÒÒ 
ParentGroup
ÒÒ '
;
ÒÒ' (
PreviousGroup
ÚÚ 
.
ÚÚ 
Entries
ÚÚ !
.
ÚÚ! "
Remove
ÚÚ" (
(
ÚÚ( )
this
ÚÚ) -
)
ÚÚ- .
;
ÚÚ. /
if
ÛÛ 
(
ÛÛ 
destination
ÛÛ 
==
ÛÛ 
null
ÛÛ #
)
ÛÛ# $
{
ÙÙ 
	_database
ıı 
.
ıı 
AddDeletedItem
ıı (
(
ıı( )
IdUuid
ıı) /
)
ıı/ 0
;
ıı0 1
return
ˆˆ 
;
ˆˆ 
}
˜˜ 
ParentGroup
¯¯ 
=
¯¯ 
destination
¯¯ %
;
¯¯% &
ParentGroup
˘˘ 
.
˘˘ 
Entries
˘˘ 
.
˘˘  
Add
˘˘  #
(
˘˘# $
this
˘˘$ (
)
˘˘( )
;
˘˘) *
}
˙˙ 	
public
¸¸ 
void
¸¸ 
CommitDelete
¸¸  
(
¸¸  !
)
¸¸! "
{
˝˝ 	
_pwEntry
˛˛ 
.
˛˛ 
ParentGroup
˛˛  
.
˛˛  !
Entries
˛˛! (
.
˛˛( )
Remove
˛˛) /
(
˛˛/ 0
_pwEntry
˛˛0 8
)
˛˛8 9
;
˛˛9 :
if
ˇˇ 
(
ˇˇ 
!
ˇˇ 
	_database
ˇˇ 
.
ˇˇ 
RecycleBinEnabled
ˇˇ ,
||
ˇˇ- /
PreviousGroup
ˇˇ0 =
.
ˇˇ= >

IsSelected
ˇˇ> H
)
ˇˇH I
	_database
ˇˇJ S
.
ˇˇS T
AddDeletedItem
ˇˇT b
(
ˇˇb c
IdUuid
ˇˇc i
)
ˇˇi j
;
ˇˇj k
}
ÄÄ 	
public
ÇÇ 
void
ÇÇ 
Save
ÇÇ 
(
ÇÇ 
)
ÇÇ 
{
ÉÉ 	
	_database
ÑÑ 
.
ÑÑ 
Save
ÑÑ 
(
ÑÑ 
)
ÑÑ 
;
ÑÑ 
}
ÖÖ 	
public
áá 
PwEntry
áá 

GetPwEntry
áá !
(
áá! "
)
áá" #
{
àà 	
return
ââ 
_pwEntry
ââ 
;
ââ 
}
ää 	
public
åå 
override
åå 
string
åå 
ToString
åå '
(
åå' (
)
åå( )
{
çç 	
return
éé 

IsSelected
éé 
?
éé 
	_resource
éé  )
.
éé) *
GetResourceValue
éé* :
(
éé: ;
$str
éé; I
)
ééI J
:
ééK L
_pwEntry
ééM U
.
ééU V"
LastModificationTime
ééV j
.
ééj k
ToString
éék s
(
éés t
$str
éét w
)
ééw x
;
ééx y
}
èè 	
}
êê 
}ëë Ë≥
BC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\GroupVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
GroupVm 
: %
NotifyPropertyChangedBase 4
,4 5
	IPwEntity6 ?
,? @
ISelectableModelA Q
{ 
public 
GroupVm 
ParentGroup "
{# $
get% (
;( )
private* 1
set2 5
;5 6
}7 8
public 
GroupVm 
PreviousGroup $
{% &
get' *
;* +
private, 3
set4 7
;7 8
}9 :
public  
ObservableCollection #
<# $
EntryVm$ +
>+ ,
Entries- 4
{ 	
get 
{ 
return 
_entries !
;! "
}# $
private 
set 
{ 
SetProperty %
(% &
ref& )
_entries* 2
,2 3
value4 9
)9 :
;: ;
}< =
} 	
public 
IEnumerable 
< 
EntryVm "
>" #

SubEntries$ .
{ 	
get 
{ 
var 

subEntries 
=  
new! $
List% )
<) *
EntryVm* 1
>1 2
(2 3
)3 4
;4 5

subEntries 
. 
AddRange #
(# $
Entries$ +
)+ ,
;, -
foreach 
( 
var 
group "
in# %
Groups& ,
), -
{ 

subEntries   
.   
AddRange   '
(  ' (
group  ( -
.  - .

SubEntries  . 8
)  8 9
;  9 :
}!! 
return## 

subEntries## !
;##! "
}$$ 
}%% 	
public''  
ObservableCollection'' #
<''# $
GroupVm''$ +
>''+ ,
Groups''- 3
{''4 5
get''6 9
;''9 :
set''; >
;''> ?
}''@ A
=''B C
new''D G 
ObservableCollection''H \
<''\ ]
GroupVm''] d
>''d e
(''e f
)''f g
;''g h
public)) 
PwUuid)) 
IdUuid)) 
=>)) 
_pwGroup))  (
?))( )
.))) *
Uuid))* .
;)). /
public** 
string** 
Id** 
=>** 
IdUuid** "
?**" #
.**# $
ToHexString**$ /
(**/ 0
)**0 1
;**1 2
public++ 
bool++ 
	IsNotRoot++ 
=>++  
ParentGroup++! ,
!=++- /
null++0 4
;++4 5
public-- 
bool-- 
ShowRestore-- 
=>--  "
	IsNotRoot--# ,
&&--- /
ParentGroup--0 ;
.--; <

IsSelected--< F
;--F G
public// 
bool// 
IsRecycleOnDelete// %
=>//& (
	_database//) 2
.//2 3
RecycleBinEnabled//3 D
&&//E G
!//H I

IsSelected//I S
&&//T V
!//W X
ParentGroup//X c
.//c d

IsSelected//d n
;//n o
public44 
bool44 

IsSelected44 
{55 	
get66 
{66 
return66 
	_database66 "
!=66# %
null66& *
&&66+ -
	_database66. 7
.667 8
RecycleBinEnabled668 I
&&66J L
	_database66M V
.66V W

RecycleBin66W a
?66a b
.66b c
Id66c e
==66f h
Id66i k
;66k l
}66m n
set77 
{88 
if99 
(99 
value99 
&&99 
_pwGroup99 %
!=99& (
null99) -
)99- .
	_database99/ 8
.998 9

RecycleBin999 C
=99D E
this99F J
;99J K
}:: 
};; 	
public== 
IOrderedEnumerable== !
<==! "
	IGrouping==" +
<==+ ,
char==, 0
,==0 1
EntryVm==2 9
>==9 :
>==: ;
EntriesZoomedOut==< L
=>==M O
from==P T
e==U V
in==W Y
Entries==Z a
group>> 
e>> 
by>> 
e>> 
.>> 
Name>> 
.>> 
ToUpper>> %
(>>% &
)>>& '
.>>' (
FirstOrDefault>>( 6
(>>6 7
)>>7 8
into>>9 =
grp>>> A
orderby?? 
grp?? 
.?? 
Key?? 
select@@ 
grp@@ 
;@@ 
publicBB 
stringBB 
NameBB 
{CC 	
getDD 
{DD 
returnDD 
_pwGroupDD !
==DD" $
nullDD% )
?DD* +
stringDD, 2
.DD2 3
EmptyDD3 8
:DD9 :
_pwGroupDD; C
.DDC D
NameDDD H
;DDH I
}DDJ K
setEE 
{EE 
_pwGroupEE 
.EE 
NameEE 
=EE  !
valueEE" '
;EE' (
}EE) *
}FF 	
publicHH 
intHH 
IconIdHH 
{II 	
getJJ 
{KK 
ifLL 
(LL 
_pwGroupLL 
?LL 
.LL 
IconIdLL $
!=LL% '
nullLL( ,
)LL, -
returnLL. 4
(LL5 6
intLL6 9
)LL9 :
_pwGroupLL; C
?LLC D
.LLD E
IconIdLLE K
;LLK L
returnMM 
-MM 
$numMM 
;MM 
}NN 
setOO 
{OO 
_pwGroupOO 
.OO 
IconIdOO !
=OO" #
(OO$ %
PwIconOO% +
)OO+ ,
valueOO, 1
;OO1 2
}OO3 4
}PP 	
publicRR 
boolRR 

IsEditModeRR 
{SS 	
getTT 
{TT 
returnTT 
_isEditModeTT $
;TT$ %
}TT& '
setUU 
{UU 
SetPropertyUU 
(UU 
refUU !
_isEditModeUU" -
,UU- .
valueUU/ 4
)UU4 5
;UU5 6
}UU7 8
}VV 	
publicXX 
boolXX 
IsMenuClosedXX  
{YY 	
getZZ 
{ZZ 
returnZZ 
_isMenuClosedZZ &
;ZZ& '
}ZZ( )
set[[ 
{[[ 
SetProperty[[ 
([[ 
ref[[ !
_isMenuClosed[[" /
,[[/ 0
value[[1 6
)[[6 7
;[[7 8
}[[9 :
}\\ 	
public^^ 
IEnumerable^^ 
<^^ 
	IPwEntity^^ $
>^^$ %

BreadCrumb^^& 0
{__ 	
get`` 
{aa 
varbb 
groupsbb 
=bb 
newbb  
Stackbb! &
<bb& '
GroupVmbb' .
>bb. /
(bb/ 0
)bb0 1
;bb1 2
varcc 
groupcc 
=cc 
thiscc  
;cc  !
whiledd 
(dd 
groupdd 
.dd 
ParentGroupdd (
!=dd) +
nulldd, 0
)dd0 1
{ee 
groupff 
=ff 
groupff !
.ff! "
ParentGroupff" -
;ff- .
groupsgg 
.gg 
Pushgg 
(gg  
groupgg  %
)gg% &
;gg& '
}hh 
returnjj 
groupsjj 
;jj 
}kk 
}ll 	
privatenn 
readonlynn 
PwGroupnn  
_pwGroupnn! )
;nn) *
privateoo 
readonlyoo 
IDatabaseServiceoo )
	_databaseoo* 3
;oo3 4
privatepp 
boolpp 
_isEditModepp  
;pp  !
privateqq 
PwEntryqq 
_reorderedEntryqq '
;qq' (
privaterr  
ObservableCollectionrr $
<rr$ %
EntryVmrr% ,
>rr, -
_entriesrr. 6
=rr7 8
newrr9 < 
ObservableCollectionrr= Q
<rrQ R
EntryVmrrR Y
>rrY Z
(rrZ [
)rr[ \
;rr\ ]
privatess 
boolss 
_isMenuClosedss "
=ss# $
truess% )
;ss) *
publicuu 
GroupVmuu 
(uu 
)uu 
{uu 
}uu 
internalww 
GroupVmww 
(ww 
PwGroupww  
pwGroupww! (
,ww( )
GroupVmww* 1
parentww2 8
,ww8 9
PwUuidww: @
recycleBinIdwwA M
=wwN O
nullwwP T
)wwT U
:wwV W
thiswwX \
(ww\ ]
pwGroupww] d
,wwd e
parentwwf l
,wwl m
DatabaseServicexx 
.xx 
Instancexx $
,xx$ %
recycleBinIdxx& 2
)xx2 3
{yy 	
}yy
 
public{{ 
GroupVm{{ 
({{ 
PwGroup{{ 
pwGroup{{ &
,{{& '
GroupVm{{( /
parent{{0 6
,{{6 7
IDatabaseService{{8 H
database{{I Q
,{{Q R
PwUuid{{S Y
recycleBinId{{Z f
={{g h
null{{i m
){{m n
{|| 	
_pwGroup}} 
=}} 
pwGroup}} 
;}} 
	_database~~ 
=~~ 
database~~  
;~~  !
ParentGroup 
= 
parent  
;  !
if
ÅÅ 
(
ÅÅ 
recycleBinId
ÅÅ 
!=
ÅÅ 
null
ÅÅ  $
&&
ÅÅ% '
_pwGroup
ÅÅ( 0
.
ÅÅ0 1
Uuid
ÅÅ1 5
.
ÅÅ5 6
Equals
ÅÅ6 <
(
ÅÅ< =
recycleBinId
ÅÅ= I
)
ÅÅI J
)
ÅÅJ K
	_database
ÅÅL U
.
ÅÅU V

RecycleBin
ÅÅV `
=
ÅÅa b
this
ÅÅc g
;
ÅÅg h
Entries
ÇÇ 
=
ÇÇ 
new
ÇÇ "
ObservableCollection
ÇÇ .
<
ÇÇ. /
EntryVm
ÇÇ/ 6
>
ÇÇ6 7
(
ÇÇ7 8
pwGroup
ÇÇ8 ?
.
ÇÇ? @
Entries
ÇÇ@ G
.
ÇÇG H
Select
ÇÇH N
(
ÇÇN O
e
ÇÇO P
=>
ÇÇQ S
new
ÇÇT W
EntryVm
ÇÇX _
(
ÇÇ_ `
e
ÇÇ` a
,
ÇÇa b
this
ÇÇc g
)
ÇÇg h
)
ÇÇh i
)
ÇÇi j
;
ÇÇj k
Entries
ÉÉ 
.
ÉÉ 
CollectionChanged
ÉÉ %
+=
ÉÉ& ('
Entries_CollectionChanged
ÉÉ) B
;
ÉÉB C
Groups
ÑÑ 
=
ÑÑ 
new
ÑÑ "
ObservableCollection
ÑÑ -
<
ÑÑ- .
GroupVm
ÑÑ. 5
>
ÑÑ5 6
(
ÑÑ6 7
pwGroup
ÑÑ7 >
.
ÑÑ> ?
Groups
ÑÑ? E
.
ÑÑE F
Select
ÑÑF L
(
ÑÑL M
g
ÑÑM N
=>
ÑÑO Q
new
ÑÑR U
GroupVm
ÑÑV ]
(
ÑÑ] ^
g
ÑÑ^ _
,
ÑÑ_ `
this
ÑÑa e
,
ÑÑe f
recycleBinId
ÑÑg s
)
ÑÑs t
)
ÑÑt u
)
ÑÑu v
;
ÑÑv w
}
ÖÖ 	
private
áá 
void
áá '
Entries_CollectionChanged
áá .
(
áá. /
object
áá/ 5
sender
áá6 <
,
áá< =.
 NotifyCollectionChangedEventArgs
áá> ^
e
áá_ `
)
áá` a
{
àà 	
switch
ââ 
(
ââ 
e
ââ 
.
ââ 
Action
ââ 
)
ââ 
{
ää 
case
ãã +
NotifyCollectionChangedAction
ãã 2
.
ãã2 3
Remove
ãã3 9
:
ãã9 :
var
åå 
oldIndex
åå  
=
åå! "
(
åå# $
uint
åå$ (
)
åå( )
e
åå* +
.
åå+ ,
OldStartingIndex
åå, <
;
åå< =
_reorderedEntry
çç $
=
çç% &
_pwGroup
çç' /
.
çç/ 0
Entries
çç0 7
.
çç7 8
GetAt
çç8 =
(
çç= >
oldIndex
çç> F
)
ççF G
;
ççG H
_pwGroup
éé 
.
éé 
Entries
éé $
.
éé$ %
RemoveAt
éé% -
(
éé- .
oldIndex
éé. 6
)
éé6 7
;
éé7 8
break
èè 
;
èè 
case
êê +
NotifyCollectionChangedAction
êê 2
.
êê2 3
Add
êê3 6
:
êê6 7
if
ëë 
(
ëë 
_reorderedEntry
ëë '
==
ëë( *
null
ëë+ /
)
ëë/ 0
_pwGroup
ëë1 9
.
ëë9 :
AddEntry
ëë: B
(
ëëB C
(
ëëC D
(
ëëD E
EntryVm
ëëE L
)
ëëL M
e
ëëN O
.
ëëO P
NewItems
ëëP X
[
ëëX Y
$num
ëëY Z
]
ëëZ [
)
ëë[ \
.
ëë\ ]

GetPwEntry
ëë] g
(
ëëg h
)
ëëh i
,
ëëi j
true
ëëk o
)
ëëo p
;
ëëp q
else
íí 
_pwGroup
íí !
.
íí! "
Entries
íí" )
.
íí) *
Insert
íí* 0
(
íí0 1
(
íí1 2
uint
íí2 6
)
íí6 7
e
íí7 8
.
íí8 9
NewStartingIndex
íí9 I
,
ííI J
_reorderedEntry
ííK Z
)
ííZ [
;
íí[ \
break
ìì 
;
ìì 
}
îî 
}
ïï 	
public
óó 
GroupVm
óó 
AddNewGroup
óó "
(
óó" #
string
óó# )
name
óó* .
=
óó/ 0
$str
óó1 3
)
óó3 4
{
òò 	
var
ôô 
pwGroup
ôô 
=
ôô 
new
ôô 
PwGroup
ôô %
(
ôô% &
true
ôô& *
,
ôô* +
true
ôô, 0
,
ôô0 1
name
ôô2 6
,
ôô6 7
PwIcon
ôô8 >
.
ôô> ?
Folder
ôô? E
)
ôôE F
;
ôôF G
_pwGroup
öö 
.
öö 
AddGroup
öö 
(
öö 
pwGroup
öö %
,
öö% &
true
öö' +
)
öö+ ,
;
öö, -
var
õõ 
newGroup
õõ 
=
õõ 
new
õõ 
GroupVm
õõ &
(
õõ& '
pwGroup
õõ' .
,
õõ. /
this
õõ0 4
)
õõ4 5
{
õõ6 7
Name
õõ7 ;
=
õõ< =
name
õõ> B
,
õõB C

IsEditMode
õõD N
=
õõO P
string
õõQ W
.
õõW X
IsNullOrEmpty
õõX e
(
õõe f
name
õõf j
)
õõj k
}
õõk l
;
õõl m
Groups
úú 
.
úú 
Add
úú 
(
úú 
newGroup
úú 
)
úú  
;
úú  !
return
ùù 
newGroup
ùù 
;
ùù 
}
ûû 	
public
†† 
EntryVm
†† 
AddNewEntry
†† "
(
††" #
)
††# $
{
°° 	
var
¢¢ 
pwEntry
¢¢ 
=
¢¢ 
new
¢¢ 
PwEntry
¢¢ %
(
¢¢% &
true
¢¢& *
,
¢¢* +
true
¢¢, 0
)
¢¢0 1
;
¢¢1 2
var
££ 
newEntry
££ 
=
££ 
new
££ 
EntryVm
££ &
(
££& '
pwEntry
££' .
,
££. /
this
££0 4
)
££4 5
{
££6 7

IsEditMode
££7 A
=
££B C
true
££D H
}
££H I
;
££I J
newEntry
§§ 
.
§§ 
GeneratePassword
§§ %
(
§§% &
)
§§& '
;
§§' (
Entries
•• 
.
•• 
Add
•• 
(
•• 
newEntry
••  
)
••  !
;
••! "
return
¶¶ 
newEntry
¶¶ 
;
¶¶ 
}
ßß 	
public
©© 
void
©© 
MarkForDelete
©© !
(
©©! "
string
©©" (
recycleBinTitle
©©) 8
)
©©8 9
{
™™ 	
if
´´ 
(
´´ 
	_database
´´ 
.
´´ 
RecycleBinEnabled
´´ +
&&
´´, .
	_database
´´/ 8
.
´´8 9

RecycleBin
´´9 C
?
´´C D
.
´´D E
IdUuid
´´E K
==
´´L N
null
´´O S
)
´´S T
	_database
¨¨ 
.
¨¨ 
CreateRecycleBin
¨¨ *
(
¨¨* +
recycleBinTitle
¨¨+ :
)
¨¨: ;
;
¨¨; <
Move
≠≠ 
(
≠≠ 
	_database
≠≠ 
.
≠≠ 
RecycleBinEnabled
≠≠ ,
&&
≠≠- /
!
≠≠0 1

IsSelected
≠≠1 ;
?
≠≠< =
	_database
≠≠> G
.
≠≠G H

RecycleBin
≠≠H R
:
≠≠S T
null
≠≠U Y
)
≠≠Y Z
;
≠≠Z [
}
ÆÆ 	
public
∞∞ 
void
∞∞ 

UndoDelete
∞∞ 
(
∞∞ 
)
∞∞  
{
±± 	
Move
≤≤ 
(
≤≤ 
PreviousGroup
≤≤ 
)
≤≤ 
;
≤≤  
}
≥≥ 	
public
µµ 
void
µµ 
Move
µµ 
(
µµ 
GroupVm
µµ  
destination
µµ! ,
)
µµ, -
{
∂∂ 	
PreviousGroup
∑∑ 
=
∑∑ 
ParentGroup
∑∑ '
;
∑∑' (
PreviousGroup
∏∏ 
.
∏∏ 
Groups
∏∏  
.
∏∏  !
Remove
∏∏! '
(
∏∏' (
this
∏∏( ,
)
∏∏, -
;
∏∏- .
PreviousGroup
ππ 
.
ππ 
_pwGroup
ππ "
.
ππ" #
Groups
ππ# )
.
ππ) *
Remove
ππ* 0
(
ππ0 1
_pwGroup
ππ1 9
)
ππ9 :
;
ππ: ;
if
∫∫ 
(
∫∫ 
destination
∫∫ 
==
∫∫ 
null
∫∫ #
)
∫∫# $
{
ªª 
	_database
ºº 
.
ºº 
AddDeletedItem
ºº (
(
ºº( )
IdUuid
ºº) /
)
ºº/ 0
;
ºº0 1
return
ΩΩ 
;
ΩΩ 
}
ææ 
ParentGroup
øø 
=
øø 
destination
øø %
;
øø% &
ParentGroup
¿¿ 
.
¿¿ 
Groups
¿¿ 
.
¿¿ 
Add
¿¿ "
(
¿¿" #
this
¿¿# '
)
¿¿' (
;
¿¿( )
ParentGroup
¡¡ 
.
¡¡ 
_pwGroup
¡¡  
.
¡¡  !
AddGroup
¡¡! )
(
¡¡) *
_pwGroup
¡¡* 2
,
¡¡2 3
true
¡¡4 8
)
¡¡8 9
;
¡¡9 :
}
¬¬ 	
public
ƒƒ 
void
ƒƒ 
CommitDelete
ƒƒ  
(
ƒƒ  !
)
ƒƒ! "
{
≈≈ 	
_pwGroup
∆∆ 
.
∆∆ 
ParentGroup
∆∆  
.
∆∆  !
Groups
∆∆! '
.
∆∆' (
Remove
∆∆( .
(
∆∆. /
_pwGroup
∆∆/ 7
)
∆∆7 8
;
∆∆8 9
if
«« 
(
«« 
	_database
«« 
.
«« 
RecycleBinEnabled
«« +
&&
««, .
!
««/ 0
PreviousGroup
««0 =
.
««= >

IsSelected
««> H
)
««H I
	_database
««J S
.
««S T

RecycleBin
««T ^
.
««^ _
_pwGroup
««_ g
.
««g h
AddGroup
««h p
(
««p q
_pwGroup
««q y
,
««y z
true
««{ 
)«« Ä
;««Ä Å
else
»» 
	_database
»» 
.
»» 
AddDeletedItem
»» )
(
»») *
IdUuid
»»* 0
)
»»0 1
;
»»1 2
}
…… 	
public
ÀÀ 
void
ÀÀ 
Save
ÀÀ 
(
ÀÀ 
)
ÀÀ 
{
ÃÃ 	
	_database
ÕÕ 
.
ÕÕ 
Save
ÕÕ 
(
ÕÕ 
)
ÕÕ 
;
ÕÕ 
}
ŒŒ 	
public
–– 
void
–– 
SortEntries
–– 
(
––  
)
––  !
{
—— 	
var
““ 
comparer
““ 
=
““ 
new
““ 
PwEntryComparer
““ .
(
““. /
PwDefs
““/ 5
.
““5 6

TitleField
““6 @
,
““@ A
true
““B F
,
““F G
false
““H M
)
““M N
;
““N O
try
”” 
{
‘‘ 
_pwGroup
’’ 
.
’’ 
Entries
’’  
.
’’  !
Sort
’’! %
(
’’% &
comparer
’’& .
)
’’. /
;
’’/ 0
Entries
÷÷ 
=
÷÷ 
new
÷÷ "
ObservableCollection
÷÷ 2
<
÷÷2 3
EntryVm
÷÷3 :
>
÷÷: ;
(
÷÷; <
Entries
÷÷< C
.
÷÷C D
OrderBy
÷÷D K
(
÷÷K L
e
÷÷L M
=>
÷÷N P
e
÷÷Q R
.
÷÷R S
Name
÷÷S W
)
÷÷W X
)
÷÷X Y
;
÷÷Y Z
}
◊◊ 
catch
ÿÿ 
(
ÿÿ 
	Exception
ÿÿ 
e
ÿÿ 
)
ÿÿ 
{
ŸŸ !
MessageDialogHelper
⁄⁄ #
.
⁄⁄# $
ShowErrorDialog
⁄⁄$ 3
(
⁄⁄3 4
e
⁄⁄4 5
)
⁄⁄5 6
;
⁄⁄6 7
}
€€ 
}
‹‹ 	
public
ﬁﬁ 
void
ﬁﬁ 

SortGroups
ﬁﬁ 
(
ﬁﬁ 
)
ﬁﬁ  
{
ﬂﬂ 	
try
‡‡ 
{
·· 
_pwGroup
‚‚ 
.
‚‚ 
SortSubGroups
‚‚ &
(
‚‚& '
false
‚‚' ,
)
‚‚, -
;
‚‚- .
Groups
„„ 
=
„„ 
new
„„ "
ObservableCollection
„„ 1
<
„„1 2
GroupVm
„„2 9
>
„„9 :
(
„„: ;
Groups
„„; A
.
„„A B
OrderBy
„„B I
(
„„I J
g
„„J K
=>
„„L N
g
„„O P
.
„„P Q
Name
„„Q U
)
„„U V
.
„„V W
ThenBy
„„W ]
(
„„] ^
g
„„^ _
=>
„„` b
g
„„c d
.
„„d e
_pwGroup
„„e m
==
„„n p
null
„„q u
)
„„u v
)
„„v w
;
„„w x
OnPropertyChanged
‰‰ !
(
‰‰! "
$str
‰‰" *
)
‰‰* +
;
‰‰+ ,
}
ÂÂ 
catch
ÊÊ 
(
ÊÊ 
	Exception
ÊÊ 
e
ÊÊ 
)
ÊÊ 
{
ÁÁ !
MessageDialogHelper
ËË #
.
ËË# $
ShowErrorDialog
ËË$ 3
(
ËË3 4
e
ËË4 5
)
ËË5 6
;
ËË6 7
}
ÈÈ 
}
ÍÍ 	
public
ÏÏ 
override
ÏÏ 
string
ÏÏ 
ToString
ÏÏ '
(
ÏÏ' (
)
ÏÏ( )
{
ÌÌ 	
return
ÓÓ 
Name
ÓÓ 
;
ÓÓ 
}
ÔÔ 	
}
 
}ÒÒ ¿
NC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\Items\SettingsNewVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
SettingsNewVm 
{ 
private		 
readonly		 
ISettingsService		 )
	_settings		* 3
;		3 4
public 
SettingsNewVm 
( 
) 
:  
this! %
(% &
SettingsService& 5
.5 6
Instance6 >
)> ?
{ 	
}
 
public 
SettingsNewVm 
( 
ISettingsService -
settings. 6
)6 7
{ 	
	_settings 
= 
settings  
;  !
} 	
public 
bool 
IsCreateSample "
{ 	
get 
{ 
return 
	_settings "
." #

GetSetting# -
<- .
bool. 2
>2 3
(3 4
$str4 <
)< =
;= >
}? @
set 
{ 
	_settings 
. 

PutSetting &
(& '
$str' /
,/ 0
value1 6
)6 7
;7 8
}9 :
} 	
public 
IEnumerable 
< 
string !
>! "
FileFormats# .
=>/ 1
new2 5
[6 7
]7 8
{8 9
$str9 <
,< =
$str> A
}A B
;B C
public 
string 
FileFormatVersion '
{ 	
get 
{ 
return 
	_settings "
." #

GetSetting# -
<- .
string. 4
>4 5
(5 6
$str6 I
)I J
;J K
}L M
set 
{ 
	_settings 
. 

PutSetting &
(& '
$str' :
,: ;
value< A
)A B
;B C
}D E
} 	
}   
}!! “3
EC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\SettingsVm.cs
	namespace		 	
ModernKeePass		
 
.		 

ViewModels		 "
{

 
public 

class 

SettingsVm 
: %
NotifyPropertyChangedBase 7
,7 8 
IHasSelectableObject9 M
{ 
private 
ListMenuItemVm 
_selectedItem ,
;, -
private 
IOrderedEnumerable "
<" #
	IGrouping# ,
<, -
string- 3
,3 4
ListMenuItemVm5 C
>C D
>D E

_menuItemsF P
;P Q
public 
IOrderedEnumerable !
<! "
	IGrouping" +
<+ ,
string, 2
,2 3
ListMenuItemVm4 B
>B C
>C D
	MenuItemsE N
{ 	
get 
{ 
return 

_menuItems #
;# $
}% &
set 
{ 
SetProperty 
( 
ref !

_menuItems" ,
,, -
value. 3
)3 4
;4 5
}6 7
} 	
public 
ISelectableModel 
SelectedItem  ,
{ 	
get 
{ 
return 
_selectedItem &
;& '
}( )
set 
{ 
if 
( 
_selectedItem !
==" $
value% *
)* +
return, 2
;2 3
if 
( 
_selectedItem !
!=" $
null% )
)) *
{ 
_selectedItem !
.! "

IsSelected" ,
=- .
false/ 4
;4 5
}   
SetProperty"" 
("" 
ref"" 
_selectedItem""  -
,""- .
(""/ 0
ListMenuItemVm""0 >
)""> ?
value""? D
)""D E
;""E F
if$$ 
($$ 
_selectedItem$$ !
!=$$" $
null$$% )
)$$) *
{%% 
_selectedItem&& !
.&&! "

IsSelected&&" ,
=&&- .
true&&/ 3
;&&3 4
}'' 
}(( 
})) 	
public++ 

SettingsVm++ 
(++ 
)++ 
:++ 
this++ "
(++" #
DatabaseService++# 2
.++2 3
Instance++3 ;
,++; <
new++= @
ResourcesService++A Q
(++Q R
)++R S
)++S T
{++U V
}++W X
public-- 

SettingsVm-- 
(-- 
IDatabaseService-- *
database--+ 3
,--3 4
IResourceService--5 E
resource--F N
)--N O
{.. 	
var// 
	menuItems// 
=// 
new//  
ObservableCollection//  4
<//4 5
ListMenuItemVm//5 C
>//C D
{00 
new11 
ListMenuItemVm11 "
{22 
Title33 
=33 
resource33 $
.33$ %
GetResourceValue33% 5
(335 6
$str336 K
)33K L
,33L M
Group44 
=44 
resource44 $
.44$ %
GetResourceValue44% 5
(445 6
$str446 T
)44T U
,44U V

SymbolIcon55 
=55  
Symbol55! '
.55' (
Add55( +
,55+ ,
PageType66 
=66 
typeof66 %
(66% &#
SettingsNewDatabasePage66& =
)66= >
,66> ?

IsSelected77 
=77  
true77! %
}88 
,88 
new99 
ListMenuItemVm99 "
{:: 
Title;; 
=;; 
resource;; $
.;;$ %
GetResourceValue;;% 5
(;;5 6
$str;;6 L
);;L M
,;;M N
Group<< 
=<< 
resource<< $
.<<$ %
GetResourceValue<<% 5
(<<5 6
$str<<6 T
)<<T U
,<<U V

SymbolIcon== 
===  
Symbol==! '
.==' (
Save==( ,
,==, -
PageType>> 
=>> 
typeof>> %
(>>% &
SettingsSavePage>>& 6
)>>6 7
}?? 
,?? 
new@@ 
ListMenuItemVm@@ "
{AA 
TitleBB 
=BB 
resourceBB $
.BB$ %
GetResourceValueBB% 5
(BB5 6
$strBB6 O
)BBO P
,BBP Q
GroupCC 
=CC 
resourceCC $
.CC$ %
GetResourceValueCC% 5
(CC5 6
$strCC6 Q
)CCQ R
,CCR S

SymbolIconDD 
=DD  
SymbolDD! '
.DD' (
SettingDD( /
,DD/ 0
PageTypeEE 
=EE 
typeofEE %
(EE% & 
SettingsDatabasePageEE& :
)EE: ;
,EE; <
	IsEnabledFF 
=FF 
databaseFF  (
.FF( )
IsOpenFF) /
}GG 
,GG 
newHH 
ListMenuItemVmHH "
{II 
TitleJJ 
=JJ 
resourceJJ $
.JJ$ %
GetResourceValueJJ% 5
(JJ5 6
$strJJ6 P
)JJP Q
,JJQ R
GroupKK 
=KK 
resourceKK $
.KK$ %
GetResourceValueKK% 5
(KK5 6
$strKK6 Q
)KKQ R
,KKR S

SymbolIconLL 
=LL  
SymbolLL! '
.LL' (
PermissionsLL( 3
,LL3 4
PageTypeMM 
=MM 
typeofMM %
(MM% & 
SettingsSecurityPageMM& :
)MM: ;
,MM; <
	IsEnabledNN 
=NN 
databaseNN  (
.NN( )
IsOpenNN) /
}OO 
}PP 
;PP 
SelectedItemQQ 
=QQ 
	menuItemsQQ $
.QQ$ %
FirstOrDefaultQQ% 3
(QQ3 4
mQQ4 5
=>QQ6 8
mQQ9 :
.QQ: ;

IsSelectedQQ; E
)QQE F
;QQF G
	MenuItemsSS 
=SS 
fromSS 
itemSS !
inSS" $
	menuItemsSS% .
groupSS/ 4
itemSS5 9
bySS: <
itemSS= A
.SSA B
GroupSSB G
intoSSH L
grpSSM P
orderbySSQ X
grpSSY \
.SS\ ]
KeySS] `
selectSSa g
grpSSh k
;SSk l
}TT 	
}UU 
}VV ’U
AC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\MainVm.cs
	namespace

 	
ModernKeePass


 
.

 

ViewModels

 "
{ 
public 

class 
MainVm 
: %
NotifyPropertyChangedBase 3
,3 4 
IHasSelectableObject5 I
{ 
private 
IOrderedEnumerable "
<" #
	IGrouping# ,
<, -
string- 3
,3 4
MainMenuItemVm5 C
>C D
>D E
_mainMenuItemsF T
;T U
private 
MainMenuItemVm 
_selectedItem ,
;, -
public 
string 
Name 
{ 
get  
;  !
}" #
=$ %
Package& -
.- .
Current. 5
.5 6
DisplayName6 A
;A B
public 
IOrderedEnumerable !
<! "
	IGrouping" +
<+ ,
string, 2
,2 3
MainMenuItemVm4 B
>B C
>C D
MainMenuItemsE R
{ 	
get 
{ 
return 
_mainMenuItems '
;' (
}) *
set 
{ 
SetProperty 
( 
ref !
_mainMenuItems" 0
,0 1
value2 7
)7 8
;8 9
}: ;
} 	
public 
ISelectableModel 
SelectedItem  ,
{ 	
get 
{ 
return 
_selectedItem &
;& '
}( )
set 
{ 
if 
( 
_selectedItem !
==" $
value% *
)* +
return, 2
;2 3
if 
( 
_selectedItem !
!=" $
null% )
)) *
{   
_selectedItem!! !
.!!! "

IsSelected!!" ,
=!!- .
false!!/ 4
;!!4 5
}"" 
SetProperty$$ 
($$ 
ref$$ 
_selectedItem$$  -
,$$- .
($$/ 0
MainMenuItemVm$$0 >
)$$> ?
value$$? D
)$$D E
;$$E F
if&& 
(&& 
_selectedItem&& !
!=&&" $
null&&% )
)&&) *
{'' 
_selectedItem(( !
.((! "

IsSelected((" ,
=((- .
true((/ 3
;((3 4
})) 
}** 
}++ 	
public-- 
MainVm-- 
(-- 
)-- 
{-- 
}-- 
internal// 
MainVm// 
(// 
Frame// 
referenceFrame// ,
,//, -
Frame//. 3
destinationFrame//4 D
)//D E
://F G
this//H L
(//L M
referenceFrame//M [
,//[ \
destinationFrame//] m
,//m n
DatabaseService00 
.00 
Instance00 $
,00$ %
new00& )
ResourcesService00* :
(00: ;
)00; <
,00< =
RecentService00> K
.00K L
Instance00L T
)00T U
{11 	
}11
 
public33 
MainVm33 
(33 
Frame33 
referenceFrame33 *
,33* +
Frame33, 1
destinationFrame332 B
,33B C
IDatabaseService33D T
database33U ]
,33] ^
IResourceService33_ o
resource33p x
,33x y
IRecentService	33z à
recent
33â è
)
33è ê
{44 	
var55 
isDatabaseOpen55 
=55  
database55! )
!=55* ,
null55- 1
&&552 4
database555 =
.55= >
IsOpen55> D
;55D E
var77 
mainMenuItems77 
=77 
new77  # 
ObservableCollection77$ 8
<778 9
MainMenuItemVm779 G
>77G H
{88 
new99 
MainMenuItemVm99 "
{:: 
Title;; 
=;; 
resource;; $
.;;$ %
GetResourceValue;;% 5
(;;5 6
$str;;6 H
);;H I
,;;I J
PageType<< 
=<< 
typeof<< %
(<<% &
OpenDatabasePage<<& 6
)<<6 7
,<<7 8
Destination== 
===  !
destinationFrame==" 2
,==2 3
	Parameter>> 
=>> 
referenceFrame>>  .
,>>. /

SymbolIcon?? 
=??  
Symbol??! '
.??' (
Page2??( -
,??- .

IsSelected@@ 
=@@  
database@@! )
!=@@* ,
null@@- 1
&&@@2 4
database@@5 =
.@@= >

IsFileOpen@@> H
&&@@I K
!@@L M
database@@M U
.@@U V
IsOpen@@V \
}AA 
,AA 
newBB 
MainMenuItemVmBB "
{CC 
TitleDD 
=DD 
resourceDD $
.DD$ %
GetResourceValueDD% 5
(DD5 6
$strDD6 G
)DDG H
,DDH I
PageTypeEE 
=EE 
typeofEE %
(EE% &
NewDatabasePageEE& 5
)EE5 6
,EE6 7
DestinationFF 
=FF  !
destinationFrameFF" 2
,FF2 3
	ParameterGG 
=GG 
referenceFrameGG  .
,GG. /

SymbolIconHH 
=HH  
SymbolHH! '
.HH' (
AddHH( +
}II 
,II 
newJJ 
MainMenuItemVmJJ "
{KK 
TitleLL 
=LL 
resourceLL $
.LL$ %
GetResourceValueLL% 5
(LL5 6
$strLL6 H
)LLH I
,LLI J
PageTypeMM 
=MM 
typeofMM %
(MM% &
SaveDatabasePageMM& 6
)MM6 7
,MM7 8
DestinationNN 
=NN  !
destinationFrameNN" 2
,NN2 3
	ParameterOO 
=OO 
referenceFrameOO  .
,OO. /

SymbolIconPP 
=PP  
SymbolPP! '
.PP' (
SavePP( ,
,PP, -

IsSelectedQQ 
=QQ  
isDatabaseOpenQQ! /
,QQ/ 0
	IsEnabledRR 
=RR 
isDatabaseOpenRR  .
}SS 
,SS 
newTT 
MainMenuItemVmTT "
{UU 
TitleVV 
=VV 
resourceVV $
.VV$ %
GetResourceValueVV% 5
(VV5 6
$strVV6 J
)VVJ K
,VVK L
PageTypeWW 
=WW 
typeofWW %
(WW% &
RecentDatabasesPageWW& 9
)WW9 :
,WW: ;
DestinationXX 
=XX  !
destinationFrameXX" 2
,XX2 3
	ParameterYY 
=YY 
referenceFrameYY  .
,YY. /

SymbolIconZZ 
=ZZ  
SymbolZZ! '
.ZZ' (
CopyZZ( ,
,ZZ, -

IsSelected[[ 
=[[  
(\\ 
database\\ !
==\\" $
null\\% )
||\\* ,
database\\- 5
.\\5 6
IsClosed\\6 >
)\\> ?
&&\\@ B
recent]] 
.]] 

EntryCount]] )
>]]* +
$num]], -
,]]- .
	IsEnabled^^ 
=^^ 
recent^^  &
.^^& '

EntryCount^^' 1
>^^2 3
$num^^4 5
}__ 
,__ 
new`` 
MainMenuItemVm`` "
{aa 
Titlebb 
=bb 
resourcebb $
.bb$ %
GetResourceValuebb% 5
(bb5 6
$strbb6 L
)bbL M
,bbM N
PageTypecc 
=cc 
typeofcc %
(cc% &
SettingsPagecc& 2
)cc2 3
,cc3 4
Destinationdd 
=dd  !
referenceFramedd" 0
,dd0 1

SymbolIconee 
=ee  
Symbolee! '
.ee' (
Settingee( /
}ff 
,ff 
newgg 
MainMenuItemVmgg "
{hh 
Titleii 
=ii 
resourceii $
.ii$ %
GetResourceValueii% 5
(ii5 6
$strii6 I
)iiI J
,iiJ K
PageTypejj 
=jj 
typeofjj %
(jj% &
	AboutPagejj& /
)jj/ 0
,jj0 1
Destinationkk 
=kk  !
destinationFramekk" 2
,kk2 3

SymbolIconll 
=ll  
Symbolll! '
.ll' (
Helpll( ,
}mm 
,mm 
newnn 
MainMenuItemVmnn "
{oo 
Titlepp 
=pp 
resourcepp $
.pp$ %
GetResourceValuepp% 5
(pp5 6
$strpp6 J
)ppJ K
,ppK L
PageTypeqq 
=qq 
typeofqq %
(qq% &

DonatePageqq& 0
)qq0 1
,qq1 2
Destinationrr 
=rr  !
destinationFramerr" 2
,rr2 3

SymbolIconss 
=ss  
Symbolss! '
.ss' (
Shopss( ,
}tt 
}uu 
;uu 
SelectedItemww 
=ww 
mainMenuItemsww (
.ww( )
FirstOrDefaultww) 7
(ww7 8
mww8 9
=>ww: <
mww= >
.ww> ?

IsSelectedww? I
)wwI J
;wwJ K
ifzz 
(zz 
databasezz 
!=zz 
nullzz  
&&zz! #
databasezz$ ,
.zz, -
IsOpenzz- 3
)zz3 4
mainMenuItems{{ 
.{{ 
Add{{ !
({{! "
new{{" %
MainMenuItemVm{{& 4
{|| 
Title}} 
=}} 
database}} $
.}}$ %
Name}}% )
,}}) *
PageType~~ 
=~~ 
typeof~~ %
(~~% &
GroupDetailPage~~& 5
)~~5 6
,~~6 7
Destination 
=  !
referenceFrame" 0
,0 1
	Parameter
ÄÄ 
=
ÄÄ 
database
ÄÄ  (
.
ÄÄ( )
	RootGroup
ÄÄ) 2
,
ÄÄ2 3
Group
ÅÅ 
=
ÅÅ 
$str
ÅÅ '
,
ÅÅ' (

SymbolIcon
ÇÇ 
=
ÇÇ  
Symbol
ÇÇ! '
.
ÇÇ' (
ProtectedDocument
ÇÇ( 9
}
ÉÉ 
)
ÉÉ 
;
ÉÉ 
MainMenuItems
ÖÖ 
=
ÖÖ 
from
ÖÖ  
item
ÖÖ! %
in
ÖÖ& (
mainMenuItems
ÖÖ) 6
group
ÖÖ7 <
item
ÖÖ= A
by
ÖÖB D
item
ÖÖE I
.
ÖÖI J
Group
ÖÖJ O
into
ÖÖP T
grp
ÖÖU X
orderby
ÖÖY `
grp
ÖÖa d
.
ÖÖd e
Key
ÖÖe h
select
ÖÖi o
grp
ÖÖp s
;
ÖÖs t
}
ÜÜ 	
}
áá 
}àà  
@C:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\NewVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
NewVm 
: 
OpenVm 
{ 
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} õ
AC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\OpenVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
OpenVm 
: %
NotifyPropertyChangedBase 2
{		 
public

 
bool

 
ShowPasswordBox

 #
=>

$ &
	_database

' 0
.

0 1

IsFileOpen

1 ;
;

; <
public 
string 
Name 
=> 
	_database '
?' (
.( )
Name) -
;- .
private 
readonly 
IDatabaseService )
	_database* 3
;3 4
public 
OpenVm 
( 
) 
: 
this 
( 
DatabaseService .
.. /
Instance/ 7
)7 8
{9 :
}; <
public 
OpenVm 
( 
IDatabaseService &
database' /
)/ 0
{ 	
	_database 
= 
database  
;  !
if 
( 
database 
== 
null  
||! #
!$ %
database% -
.- .

IsFileOpen. 8
)8 9
return: @
;@ A
OpenFile 
( 
database 
. 
DatabaseFile *
)* +
;+ ,
} 	
public 
void 
OpenFile 
( 
StorageFile (
file) -
)- .
{ 	
OpenFile 
( 
file 
, 
RecentService (
.( )
Instance) 1
)1 2
;2 3
} 	
public 
void 
OpenFile 
( 
StorageFile (
file) -
,- .
IRecentService/ =
recent> D
)D E
{ 	
	_database   
.   
DatabaseFile   "
=  # $
file  % )
;  ) *
OnPropertyChanged!! 
(!! 
$str!! $
)!!$ %
;!!% &
OnPropertyChanged"" 
("" 
$str"" /
)""/ 0
;""0 1
AddToRecentList## 
(## 
file##  
,##  !
recent##" (
)##( )
;##) *
}$$ 	
private&& 
void&& 
AddToRecentList&& $
(&&$ %
StorageFile&&% 0
file&&1 5
,&&5 6
IRecentService&&7 E
recent&&F L
)&&L M
{'' 	
recent(( 
.(( 
Add(( 
((( 
file(( 
,(( 
file(( !
.((! "
DisplayName((" -
)((- .
;((. /
})) 	
}** 
}++ µ
CC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\RecentVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
RecentVm 
: %
NotifyPropertyChangedBase 5
,5 6 
IHasSelectableObject7 K
{		 
private

 
readonly

 
IRecentService

 '
_recent

( /
;

/ 0
private 
ISelectableModel  
_selectedItem! .
;. /
private  
ObservableCollection $
<$ %
IRecentItem% 0
>0 1
_recentItems2 >
=? @
newA D 
ObservableCollectionE Y
<Y Z
IRecentItemZ e
>e f
(f g
)g h
;h i
public  
ObservableCollection #
<# $
IRecentItem$ /
>/ 0
RecentItems1 <
{ 	
get 
{ 
return 
_recentItems %
;% &
}' (
set 
{ 
SetProperty 
( 
ref !
_recentItems" .
,. /
value0 5
)5 6
;6 7
}8 9
} 	
public 
ISelectableModel 
SelectedItem  ,
{ 	
get 
{ 
return 
_selectedItem &
;& '
}( )
set 
{ 
if 
( 
_selectedItem !
==" $
value% *
)* +
return, 2
;2 3
if 
( 
_selectedItem !
!=" $
null% )
)) *
{ 
_selectedItem !
.! "

IsSelected" ,
=- .
false/ 4
;4 5
} 
SetProperty 
( 
ref 
_selectedItem  -
,- .
value/ 4
)4 5
;5 6
if!! 
(!! 
_selectedItem!! !
==!!" $
null!!% )
)!!) *
return!!+ 1
;!!1 2
_selectedItem"" 
."" 

IsSelected"" (
="") *
true""+ /
;""/ 0
}## 
}$$ 	
public&& 
RecentVm&& 
(&& 
)&& 
:&& 
this&&  
(&&! "
RecentService&&" /
.&&/ 0
Instance&&0 8
)&&8 9
{'' 	
}''
 
public)) 
RecentVm)) 
()) 
IRecentService)) &
recent))' -
)))- .
{** 	
_recent++ 
=++ 
recent++ 
;++ 
RecentItems,, 
=,, 
_recent,, !
.,,! "
GetAllFiles,," -
(,,- .
),,. /
;,,/ 0
if-- 
(-- 
RecentItems-- 
.-- 
Count-- !
>--" #
$num--$ %
)--% &
SelectedItem.. 
=.. 
RecentItems.. *
[..* +
$num..+ ,
].., -
as... 0
RecentItemVm..1 =
;..= >
}// 	
public11 
void11 
ClearAll11 
(11 
)11 
{22 	
_recent33 
.33 
ClearAll33 
(33 
)33 
;33 
RecentItems44 
.44 
Clear44 
(44 
)44 
;44  
}55 	
}66 
}77 ∆
AC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\SaveVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
SaveVm 
{		 
private

 
readonly

 
IDatabaseService

 )
	_database

* 3
;

3 4
public 
SaveVm 
( 
) 
: 
this 
( 
DatabaseService .
.. /
Instance/ 7
)7 8
{9 :
}; <
public 
SaveVm 
( 
IDatabaseService &
database' /
)/ 0
{ 	
	_database 
= 
database  
;  !
} 	
public 
async 
Task 
Save 
( 
bool #
close$ )
=* +
true, 0
)0 1
{ 	
	_database 
. 
Save 
( 
) 
; 
if 
( 
close 
) 
await 
	_database 
.  
Close  %
(% &
)& '
;' (
} 	
public 
void 
Save 
( 
StorageFile $
file% )
)) *
{ 	
	_database 
. 
Save 
( 
file 
)  
;  !
} 	
} 
} ¸@
SC:\Sources\Other\ModernKeePass\ModernKeePass\ViewModels\Items\SettingsDatabaseVm.cs
	namespace 	
ModernKeePass
 
. 

ViewModels "
{ 
public 

class 
SettingsDatabaseVm #
:# $%
NotifyPropertyChangedBase% >
,> ? 
IHasSelectableObject@ T
{ 
private 
readonly 
IDatabaseService )
	_database* 3
;3 4
private 
GroupVm 
_selectedItem %
;% &
public 
bool 
HasRecycleBin !
{ 	
get 
{ 
return 
	_database "
." #
RecycleBinEnabled# 4
;4 5
}6 7
set 
{ 
	_database 
. 
RecycleBinEnabled +
=, -
value. 3
;3 4
OnPropertyChanged !
(! "
$str" 1
)1 2
;2 3
} 
} 	
public 
bool 
IsNewRecycleBin #
{ 	
get   
{   
return   
	_database   "
.  " #

RecycleBin  # -
==  . 0
null  1 5
;  5 6
}  7 8
set!! 
{"" 
if## 
(## 
value## 
)## 
	_database## $
.##$ %

RecycleBin##% /
=##0 1
null##2 6
;##6 7
}$$ 
}%% 	
public''  
ObservableCollection'' #
<''# $
GroupVm''$ +
>''+ ,
Groups''- 3
{''4 5
get''6 9
;''9 :
set''; >
;''> ?
}''@ A
public)) 
IEnumerable)) 
<)) 
string)) !
>))! "
Ciphers))# *
{** 	
get++ 
{,, 
for-- 
(-- 
var-- 
inx-- 
=-- 
$num--  
;--  !
inx--" %
<--& '

CipherPool--( 2
.--2 3

GlobalPool--3 =
.--= >
EngineCount--> I
;--I J
inx--K N
++--N P
)--P Q
{.. 
yield// 
return//  

CipherPool//! +
.//+ ,

GlobalPool//, 6
[//6 7
inx//7 :
]//: ;
.//; <
DisplayName//< G
;//G H
}00 
}11 
}22 	
public44 
int44 
CipherIndex44 
{55 	
get66 
{77 
for88 
(88 
var88 
inx88 
=88 
$num88  
;88  !
inx88" %
<88& '

CipherPool88( 2
.882 3

GlobalPool883 =
.88= >
EngineCount88> I
;88I J
++88K M
inx88M P
)88P Q
{99 
if:: 
(:: 

CipherPool:: "
.::" #

GlobalPool::# -
[::- .
inx::. 1
]::1 2
.::2 3

CipherUuid::3 =
.::= >
Equals::> D
(::D E
	_database::E N
.::N O

DataCipher::O Y
)::Y Z
)::Z [
return::\ b
inx::c f
;::f g
};; 
return<< 
-<< 
$num<< 
;<< 
}== 
set>> 
{>> 
	_database>> 
.>> 

DataCipher>> &
=>>' (

CipherPool>>) 3
.>>3 4

GlobalPool>>4 >
[>>> ?
value>>? D
]>>D E
.>>E F

CipherUuid>>F P
;>>P Q
}>>R S
}?? 	
publicAA 
IEnumerableAA 
<AA 
stringAA !
>AA! "
CompressionsAA# /
=>AA0 2
EnumAA3 7
.AA7 8
GetNamesAA8 @
(AA@ A
typeofAAA G
(AAG H"
PwCompressionAlgorithmAAH ^
)AA^ _
)AA_ `
.AA` a
TakeAAa e
(AAe f
(AAf g
intAAg j
)AAj k#
PwCompressionAlgorithm	AAk Å
.
AAÅ Ç
Count
AAÇ á
)
AAá à
;
AAà â
publicCC 
stringCC 
CompressionNameCC %
{DD 	
getEE 
{EE 
returnEE 
EnumEE 
.EE 
GetNameEE %
(EE% &
typeofEE& ,
(EE, -"
PwCompressionAlgorithmEE- C
)EEC D
,EED E
	_databaseEEF O
.EEO P 
CompressionAlgorithmEEP d
)EEd e
;EEe f
}EEg h
setFF 
{FF 
	_databaseFF 
.FF  
CompressionAlgorithmFF 0
=FF1 2
(FF3 4"
PwCompressionAlgorithmFF4 J
)FFJ K
EnumFFK O
.FFO P
ParseFFP U
(FFU V
typeofFFV \
(FF\ ]"
PwCompressionAlgorithmFF] s
)FFs t
,FFt u
valueFFv {
)FF{ |
;FF| }
}FF~ 
}GG 	
publicHH 
IEnumerableHH 
<HH 
stringHH !
>HH! "
KeyDerivationsHH# 1
=>HH2 4
KdfPoolHH5 <
.HH< =
EnginesHH= D
.HHD E
SelectHHE K
(HHK L
eHHL M
=>HHN P
eHHQ R
.HHR S
NameHHS W
)HHW X
;HHX Y
publicJJ 
stringJJ 
KeyDerivationNameJJ '
{KK 	
getLL 
{LL 
returnLL 
KdfPoolLL  
.LL  !
GetLL! $
(LL$ %
	_databaseLL% .
.LL. /
KeyDerivationLL/ <
.LL< =
KdfUuidLL= D
)LLD E
.LLE F
NameLLF J
;LLJ K
}LLL M
setMM 
{MM 
	_databaseMM 
.MM 
KeyDerivationMM )
=MM* +
KdfPoolMM, 3
.MM3 4
EnginesMM4 ;
.MM; <
FirstOrDefaultMM< J
(MMJ K
eMMK L
=>MMM O
eMMP Q
.MMQ R
NameMMR V
==MMW Y
valueMMZ _
)MM_ `
?MM` a
.MMa b 
GetDefaultParametersMMb v
(MMv w
)MMw x
;MMx y
}MMz {
}NN 	
publicPP 
ISelectableModelPP 
SelectedItemPP  ,
{QQ 	
getRR 
{RR 
returnRR 
GroupsRR 
.RR  
FirstOrDefaultRR  .
(RR. /
gRR/ 0
=>RR1 3
gRR4 5
.RR5 6

IsSelectedRR6 @
)RR@ A
;RRA B
}RRC D
setSS 
{TT 
ifUU 
(UU 
_selectedItemUU !
==UU" $
valueUU% *
||UU+ -
IsNewRecycleBinUU. =
)UU= >
returnUU? E
;UUE F
ifVV 
(VV 
_selectedItemVV !
!=VV" $
nullVV% )
)VV) *
{WW 
_selectedItemXX !
.XX! "

IsSelectedXX" ,
=XX- .
falseXX/ 4
;XX4 5
}YY 
SetProperty[[ 
([[ 
ref[[ 
_selectedItem[[  -
,[[- .
([[/ 0
GroupVm[[0 7
)[[7 8
value[[8 =
)[[= >
;[[> ?
if]] 
(]] 
_selectedItem]] !
!=]]" $
null]]% )
)]]) *
{^^ 
_selectedItem__ !
.__! "

IsSelected__" ,
=__- .
true__/ 3
;__3 4
}`` 
}aa 
}bb 	
publicdd 
SettingsDatabaseVmdd !
(dd! "
)dd" #
:dd$ %
thisdd& *
(dd* +
DatabaseServicedd+ :
.dd: ;
Instancedd; C
)ddC D
{ddE F
}ddG H
publicff 
SettingsDatabaseVmff !
(ff! "
IDatabaseServiceff" 2
databaseff3 ;
)ff; <
{gg 	
	_databasehh 
=hh 
databasehh  
;hh  !
Groupsii 
=ii 
	_databaseii 
?ii 
.ii  
	RootGroupii  )
.ii) *
Groupsii* 0
;ii0 1
}jj 	
}kk 
}ll ÙT
`C:\Sources\Other\ModernKeePass\ModernKeePass\Views\UserControls\HamburgerMenuUserControl.xaml.cs
	namespace 	
ModernKeePass
 
. 
Views 
. 
UserControls *
{		 
public

 

sealed

 
partial

 
class

 $
HamburgerMenuUserControl

  8
{ 
public $
HamburgerMenuUserControl '
(' (
)( )
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
public 
string 
HeaderLabel !
{ 	
get 
{ 
return 
( 
string  
)  !
GetValue! )
() *
HeaderLabelProperty* =
)= >
;> ?
}@ A
set 
{ 
SetValue 
( 
HeaderLabelProperty .
,. /
value0 5
)5 6
;6 7
}8 9
} 	
public 
static 
readonly 
DependencyProperty 1
HeaderLabelProperty2 E
=F G
DependencyProperty 
. 
Register '
(' (
$str 
, 
typeof 
( 
string 
) 
, 
typeof 
( $
HamburgerMenuUserControl /
)/ 0
,0 1
new 
PropertyMetadata $
($ %
$str% -
,- .
(/ 0
o0 1
,1 2
args3 7
)7 8
=>9 ;
{< =
}> ?
)? @
)@ A
;A B
public 
string 
ButtonLabel !
{ 	
get 
{ 
return 
( 
string  
)  !
GetValue! )
() *
ButtonLabelProperty* =
)= >
;> ?
}@ A
set   
{   
SetValue   
(   
ButtonLabelProperty   .
,  . /
value  0 5
)  5 6
;  6 7
}  8 9
}!! 	
public"" 
static"" 
readonly"" 
DependencyProperty"" 1
ButtonLabelProperty""2 E
=""F G
DependencyProperty## 
.## 
Register## '
(##' (
$str$$ 
,$$ 
typeof%% 
(%% 
string%% 
)%% 
,%% 
typeof&& 
(&& $
HamburgerMenuUserControl&& /
)&&/ 0
,&&0 1
new'' 
PropertyMetadata'' $
(''$ %
$str''% -
,''- .
(''/ 0
o''0 1
,''1 2
args''3 7
)''7 8
=>''9 ;
{''< =
}''> ?
)''? @
)''@ A
;''A B
public)) 
string)) 
DisplayMemberPath)) '
{** 	
get++ 
{++ 
return++ 
(++ 
string++  
)++  !
GetValue++! )
(++) *%
DisplayMemberPathProperty++* C
)++C D
;++D E
}++F G
set,, 
{,, 
SetValue,, 
(,, %
DisplayMemberPathProperty,, 4
,,,4 5
value,,6 ;
),,; <
;,,< =
},,> ?
}-- 	
public.. 
static.. 
readonly.. 
DependencyProperty.. 1%
DisplayMemberPathProperty..2 K
=..L M
DependencyProperty// 
.// 
Register// '
(//' (
$str00 #
,00# $
typeof11 
(11 
string11 
)11 
,11 
typeof22 
(22 $
HamburgerMenuUserControl22 /
)22/ 0
,220 1
new33 
PropertyMetadata33 $
(33$ %
$str33% ,
,33, -
(33. /
o33/ 0
,330 1
args332 6
)336 7
=>338 :
{33; <
}33= >
)33> ?
)33? @
;33@ A
public55 
object55 
ResizeTarget55 "
{66 	
get77 
{77 
return77 
GetValue77 !
(77! " 
ResizeTargetProperty77" 6
)776 7
;777 8
}779 :
set88 
{88 
SetValue88 
(88  
ResizeTargetProperty88 /
,88/ 0
value881 6
)886 7
;887 8
}889 :
}99 	
public:: 
static:: 
readonly:: 
DependencyProperty:: 1 
ResizeTargetProperty::2 F
=::G H
DependencyProperty;; 
.;; 
Register;; '
(;;' (
$str<< 
,<< 
typeof== 
(== 
object== 
)== 
,== 
typeof>> 
(>> $
HamburgerMenuUserControl>> /
)>>/ 0
,>>0 1
new?? 
PropertyMetadata?? $
(??$ %
null??% )
,??) *
(??+ ,
o??, -
,??- .
args??/ 3
)??3 4
=>??5 7
{??8 9
}??: ;
)??; <
)??< =
;??= >
publicAA 

VisibilityAA 
IsButtonVisibleAA )
{BB 	
getCC 
{CC 
returnCC 
(CC 

VisibilityCC $
)CC$ %
GetValueCC% -
(CC- .#
IsButtonVisiblePropertyCC. E
)CCE F
;CCF G
}CCH I
setDD 
{DD 
SetValueDD 
(DD #
IsButtonVisiblePropertyDD 2
,DD2 3
valueDD4 9
)DD9 :
;DD: ;
}DD< =
}EE 	
publicFF 
staticFF 
readonlyFF 
DependencyPropertyFF 1#
IsButtonVisiblePropertyFF2 I
=FFJ K
DependencyPropertyGG 
.GG 
RegisterGG '
(GG' (
$strHH !
,HH! "
typeofII 
(II 

VisibilityII !
)II! "
,II" #
typeofJJ 
(JJ $
HamburgerMenuUserControlJJ /
)JJ/ 0
,JJ0 1
newKK 
PropertyMetadataKK $
(KK$ %

VisibilityKK% /
.KK/ 0
	CollapsedKK0 9
,KK9 :
(KK; <
oKK< =
,KK= >
argsKK? C
)KKC D
=>KKE G
{KKH I
}KKJ K
)KKK L
)KKL M
;KKM N
publicMM 
IEnumerableMM 
<MM 
	IPwEntityMM $
>MM$ %
ItemsSourceMM& 1
{NN 	
getOO 
{OO 
returnOO 
(OO 
IEnumerableOO %
<OO% &
	IPwEntityOO& /
>OO/ 0
)OO0 1
GetValueOO1 9
(OO9 :
ItemsSourcePropertyOO: M
)OOM N
;OON O
}OOP Q
setPP 
{PP 
SetValuePP 
(PP 
ItemsSourcePropertyPP .
,PP. /
valuePP0 5
)PP5 6
;PP6 7
}PP8 9
}QQ 	
publicSS 
staticSS 
readonlySS 
DependencyPropertySS 1
ItemsSourcePropertySS2 E
=SSF G
DependencyPropertyTT 
.TT 
RegisterTT '
(TT' (
$strUU 
,UU 
typeofVV 
(VV 
IEnumerableVV "
<VV" #
	IPwEntityVV# ,
>VV, -
)VV- .
,VV. /
typeofWW 
(WW $
HamburgerMenuUserControlWW /
)WW/ 0
,WW0 1
newXX 
PropertyMetadataXX $
(XX$ %
newXX% (
ListXX) -
<XX- .
	IPwEntityXX. 7
>XX7 8
(XX8 9
)XX9 :
,XX: ;
(XX< =
oXX= >
,XX> ?
argsXX@ D
)XXD E
=>XXF H
{XXI J
}XXK L
)XXL M
)XXM N
;XXN O
publicZZ 
objectZZ 
SelectedItemZZ "
{[[ 	
get\\ 
{\\ 
return\\ 
GetValue\\ !
(\\! " 
SelectedItemProperty\\" 6
)\\6 7
;\\7 8
}\\9 :
set]] 
{]] 
SetValue]] 
(]]  
SelectedItemProperty]] /
,]]/ 0
value]]1 6
)]]6 7
;]]7 8
}]]9 :
}^^ 	
public__ 
static__ 
readonly__ 
DependencyProperty__ 1 
SelectedItemProperty__2 F
=__G H
DependencyProperty`` 
.`` 
Register`` '
(``' (
$straa 
,aa 
typeofbb 
(bb 
objectbb 
)bb 
,bb 
typeofcc 
(cc $
HamburgerMenuUserControlcc /
)cc/ 0
,cc0 1
newdd 
PropertyMetadatadd $
(dd$ %
nulldd% )
,dd) *
(dd+ ,
odd, -
,dd- .
argsdd/ 3
)dd3 4
=>dd5 7
{dd8 9
}dd: ;
)dd; <
)dd< =
;dd= >
publicff 
eventff (
SelectionChangedEventHandlerff 1
SelectionChangedff2 B
;ffB C
publicgg 
delegategg 
voidgg (
SelectionChangedEventHandlergg 9
(gg9 :
objectgg: @
senderggA G
,ggG H%
SelectionChangedEventArgsggI b
eggc d
)ggd e
;gge f
privatehh 
voidhh '
Selector_OnSelectionChangedhh 0
(hh0 1
objecthh1 7
senderhh8 >
,hh> ?%
SelectionChangedEventArgshh@ Y
ehhZ [
)hh[ \
{ii 	
SelectionChangedjj 
?jj 
.jj 
Invokejj $
(jj$ %
senderjj% +
,jj+ ,
ejj- .
)jj. /
;jj/ 0
}kk 	
publicmm 
eventmm %
ButtonClickedEventHandlermm .
ButtonClickedmm/ <
;mm< =
publicnn 
delegatenn 
voidnn %
ButtonClickedEventHandlernn 6
(nn6 7
objectnn7 =
sendernn> D
,nnD E
RoutedEventArgsnnF U
ennV W
)nnW X
;nnX Y
privateoo 
voidoo 
ButtonBase_OnClickoo '
(oo' (
objectoo( .
senderoo/ 5
,oo5 6
RoutedEventArgsoo7 F
eooG H
)ooH I
{pp 	
ButtonClickedqq 
?qq 
.qq 
Invokeqq !
(qq! "
senderqq" (
,qq( )
eqq* +
)qq+ ,
;qq, -
}rr 	
}ss 
}tt 