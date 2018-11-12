/* 过滤所有手机号：去重、排序、去无效，后逗号分隔 */
var SENDTYPE_SMS = '' || 'SMS';
var re_yd = "(13[4-9]|147|159|158|150|151|152|157|182|183|187|188)\\d{8}";
var re_lt = "(130|131|132|145|156|155|185|186)\\d{8}";
var re_cdma = "(133|153|180|181|189)\\d{8}";
var re_nf = "0((?:20|21|23|25|27|28|29|510|512|513|516|571|573|574|575|576|577|579|591|595|731|755|757|769|898)[1-9]\\d{7}|(?:397|483|511|514|515|517|518|519|523|527|550|551|552|553|554|555|556|557|558|559|561|562|563|564|565|566|570|572|578|580|592|593|594|596|597|598|599|660|661|662|663|668|691|692|701|710|711|712|713|714|715|716|717|718|719|722|724|728|730|732|733|734|735|736|737|738|739|743|744|745|746|750|751|752|753|754|756|758|759|760|762|763|766|768|770|771|772|773|774|775|776|777|778|779|790|791|792|793|794|795|796|797|798|799|812|813|816|817|818|825|826|827|830|831|832|833|834|835|836|837|838|839|851|852|853|854|855|856|857|858|859|870|871|872|873|874|875|876|877|878|879|883|886|887|888|890|891|892|893|894|895|896|897|899|901|902|903|906|908|909|910|911|912|913|914|915|916|917|919|930|931|932|933|934|935|936|937|938|939|941|943|951|952|953|954|955|970|971|972|973|974|975|976|977|979|990|991|992|993|994|995|996|997|998|999)[1-9]\\d{6,7})";
var re_bf = "0((?:10|22|24|311|371|377|379|411|431|451|531|532)[1-9]\\d{7}|(?:310|312|313|314|315|316|317|318|319|335|349|350|351|352|353|354|355|356|357|358|359|370|372|373|374|375|376|378|391|392|393|394|395|396|398|410|412|413|414|415|416|417|418|419|421|427|429|432|433|434|435|436|437|438|439|440|448|452|453|454|455|456|457|458|459|464|467|468|469|470|471|472|473|474|475|476|477|478|479|482|530|533|534|535|536|537|538|539|543|546|631|632|633|634|635)[1-9]\\d{6,7})";
var re_gj = "00(?:1246|1268|1345|1441|1473|1670|1671|1758|1784|1809|212|213|216|220|221|222|223|225|226|227|228|229|230|231|232|233|234|235|236|237|240|241|242|243|245|248|250|251|254|255|256|257|258|260|261|262|263|264|265|266|267|268|297|298|299|350|351|352|353|354|355|356|357|358|359|370|371|372|373|374|375|376|377|380|381|385|386|387|389|420|421|423|503|507|590|591|594|595|596|670|673|678|679|687|689|852|853|855|856|880|886|960|961|962|963|965|966|967|968|971|972|973|974|976|977|992|993|994|995|996|998|20|27|30|31|32|33|34|36|39|40|41|43|44|45|46|47|48|49|51|52|53|54|55|56|58|60|61|62|63|64|65|66|81|82|84|86|90|91|92|93|94|98|1|7)\\d{3,15}";
function checkSMSCount(inputid,spanid) {
    var obj = document.getElementById(inputid);
    obj.value = obj.value.replace(eval('/(?:^|\\D|\\b)(0086|86)/g'), ',');
    obj.value = sortMobs(obj.value);
    var num = 0;
    num = mobCount(obj.value, selectYYS(SENDTYPE_SMS));
    document.getElementById(spanid).innerHTML = num;
}
//号码函数公用---------------------------------------------------------//
function selectYYS(s) {
    switch (s) {
        case 'ALL':
            return ['yd', 'lt', 'nf', 'bf', 'gj', 'cdma'];
            break;
        case 'SMS':
            return ['yd', 'lt', 'nf', 'bf', 'gj', 'cdma'];
            break;
        case 'LONGSMS':
            return ['yd', 'lt', 'nf', 'bf', 'cdma'];
            break;
        case 'PUSH':
            return ['yd', 'lt', 'cdma'];
            break;
        case 'MMS':
            return ['yd', 'lt', 'nf', 'bf', 'cdma'];
            break;
    }
}function sortMobs(mobs) {
    mobs = mobs + ",";
    mobs = mobs.replace(/\D+/g, ',');
    var arrMobs = mobs.split(",");
    mobs = arrMobs.sort().toString() + ",";
    mobs = mobs.replace(/(\b\d+,\b)\1+/g, "$1");
    return mobs.replace(/^\D+|\D+$/g, "");
}function mobCount(mobs, arrYYS) {
    var num = 0;
    for (var i = 0; i < arrYYS.length; i++) {
        var arrMobs = null;
        switch (arrYYS[i]) {
            case 'yd':
                arrMobs = mobs.match(eval("/(?:^|\\D|\\b)(" + re_yd + ")(?:$|\\D|\\b)/g"));
                break;
            case 'lt':
                arrMobs = mobs.match(eval("/(?:^|\\D|\\b)(" + re_lt + ")(?:$|\\D|\\b)/g"));
                break;
            case 'cdma':
                arrMobs = mobs.match(eval("/(?:^|\\D|\\b)(" + re_cdma + ")(?:$|\\D|\\b)/g"));
                break;
            case 'nf':
                arrMobs = mobs.match(eval("/(?:^|\\D|\\b)(" + re_nf + ")(?:$|\\D|\\b)/g"));
                break;
            case 'bf':
                arrMobs = mobs.match(eval("/(?:^|\\D|\\b)(" + re_bf + ")(?:$|\\D|\\b)/g"));
                break;
            case 'gj':
                arrMobs = mobs.match(eval("/(?:^|\\D|\\b)(" + re_gj + ")(?:$|\\D|\\b)/g"));
                break;
        }
        if (arrMobs != null)
            num += arrMobs.length;
    }

    return num;
}

/* 邮箱批量操作 */
var re_mail = "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$";
function checkEmailCount(inputid, spanid) {
    var obj = document.getElementById(inputid);
    obj.value = obj.value.replace(eval('/\\s/g'), ',');
    obj.value = sortMails(obj.value);
    var num = 0;
    num = mailCount(obj);
    document.getElementById(spanid).innerHTML = num;
}function sortMails(mobs) {
    var arrMobs = mobs.split(",");
    mobs = arrMobs.sort().toString();
    return mobs;
}function mailCount(obj) {
    var num = 0;
    var mobs = obj.value;
    var arrMobs = mobs.split(",");
    obj.value = "";
    var c = "";
    for (i = 0; i < arrMobs.length; i++)
    {
        if (arrMobs[i].match(re_mail) != null) {
            num++;
            obj.value += c + arrMobs[i];
            c = ",";
        }
    }
    return num;
}