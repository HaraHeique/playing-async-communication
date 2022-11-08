# "Brincando" com comunicação assíncrona de API's

Comunicação entre API's pode ser realizada de duas formas: síncrona e assíncrona. Na síncrona as chamadas são bloqueantes e em tempo real, do qual a resposta é resolvida de forma imediata. Em contrapartida, as chamadas assíncronas não são bloqueantes, onde todo processamento é realizado em segundo plano e respondido em tempo diferente da chamada realizada.

Uma das formas de performar comunicação entre API's usando comunicação assíncrona é a partir de messages brokers, que basicamente são servidores capazes de armazenar mensagens provenientes de componentes chamados producers, e que em algum momento no futuro os componentes chamados consumers irão consumir essas mensagens. Ambos componentes, producers e consumers, podem ser API's.

