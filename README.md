# Mobile Platformer
В данном проекте я занимался созданием ИИ. Всего было создано 6 разных типов противников, которых можно разделить на две категории -
атакующие игрока и "атакующие пустоту".<br>
Каждый противник имеет минимум два рейкаста для принятия решений. Для примера разберем поведение скелета.
<br>
<img src="/Images/skeleton.gif"></img>
<br>
Скелет имеет два рейкаста: красный вертикальный и белый горизонтальный. Оба рейкаста находятся в нижней части коллайдера со стороны направления движения.
<br>
Красный рейкаст смотрит, есть ли под ним поверхность, пригодная для ходьбы, а белый коллайдер смотрит, нет ли перед ним непроходимых препятствий. В противных случаях скелет разворачивается и идет в другую сторону.
<br>
<img src="/Images/skeleton.png"></img>
<br>
Скелет игнорирует игрока, но наносит ему урон при прикосновении.
<br>
<img src="/Images/orc.png"></img>
<br>
У орка добавляется дополнительный горизональный рейкаст, смотрящий по направлению движения. Если этот рейкаст натыкается на игрока, орк переходит в состояние атаки.
<br>
<img src="/Images/orc.gif"></img>
<br>
При помощи этих трех базовых рейкастов в игре сделаны различные виды противников.
<br>
<img src="/Images/stationary.gif"></img>
<br>
<img src="/Images/crab.gif"></img>
<br>
<img src="/Images/ranged.gif"></img>
<br>
<img src="/Images/charge.gif"></img>
<br>
