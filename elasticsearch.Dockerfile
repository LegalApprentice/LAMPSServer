#FROM docker.elastic.co/elasticsearch/elasticsearch:7.17.1
FROM docker.elastic.co/elasticsearch/elasticsearch:7.17.10

ENV discovery.type=single-node
ENV xpack.security.enabled=false

EXPOSE 9200
EXPOSE 9300

# sudo docker build -t elasticsearch -f elasticsearch.Dockerfile .
# sudo docker run -it --rm -p 9200:9200 --name elasticsearch elasticsearch
# docker exec -it elasticsearch bash


# docker run -d -p 9200:9200 --name elasticsearch elasticsearch
# docker run --name es-node01 --net elastic -p 9200:9200 -p 9300:9300 -t docker.elastic.co/elasticsearch/elasticsearch:8.1.3
