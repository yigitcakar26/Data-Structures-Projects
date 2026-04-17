import networkx as nx
import matplotlib.pyplot as plt
import random
import heapq

# 4.a
# Graf oluşturma
graph = nx.Graph()  # Yönsüz graf (Graph)

# Düğümler ve meslek bilgileri
nodes = {
    1: "John, Tamirci",
    2: "Olivia, Doktor",
    3: "Celine, Mühendis",
    4: "Winston, Politikacı",
    5: "Jack, Profesör",
    6: "Chloe, Mimar"
}

# Düğümleri ekle
for node, label in nodes.items():
    graph.add_node(node, label=label)

# Kenarları ve ağırlıkları tanımla
edges = [
    (1, 2, 7),  # John <-> Olivia (7)
    (1, 5, 9),  # John <-> Jack (9)
    (1, 6, 7),  # John <-> Chloe (7)
    (2, 3, 8),  # Olivia <-> Celine (8)
    (2, 5, 4),  # Olivia <-> Jack (4)
    (3, 5, 5),  # Celine <-> Jack (5)
    (3, 4, 6),  # Celine <-> Winston (6)
    (4, 5, 7),  # Winston <-> Jack (7)
    (5, 6, 5),  # Jack <-> Chloe (5)
    (4, 6, 11)  # Winston <-> Chloe (11)
]

# Kenarları ekle
for u, v, weight in edges:
    graph.add_edge(u, v, weight=weight)

# Düğümleri görselleştirme pozisyonu
pos = {
    1: (-1, 0),    # John
    2: (0, 1),     # Olivia
    3: (1, 1),     # Celine
    4: (1, -1),    # Winston
    5: (0, 0),     # Jack (Merkezde)
    6: (-1, -1)    # Chloe
}

plt.figure(figsize=(10, 8))

# Düğümleri çiz
nx.draw_networkx_nodes(graph, pos, node_size=2000, node_color="lightblue")
nx.draw_networkx_labels(graph, pos, labels=nx.get_node_attributes(graph, "label"), font_size=10)

# Kenarları ve ağırlıkları çiz
nx.draw_networkx_edges(graph, pos, edge_color="gray")
nx.draw_networkx_edge_labels(graph, pos, edge_labels=nx.get_edge_attributes(graph, "weight"))

plt.title("Sosyal Ağ Grafı", fontsize=15)
plt.axis("off")
plt.show()

# 4.b
# Her düğümden diğer düğümlere olan en kısa mesafeleri hesapla
shortest_paths = {}

for source in graph.nodes():
    # Dijkstra algoritmasını kullanarak en kısa yolları ve mesafeleri hesapla
    lengths, paths = nx.single_source_dijkstra(graph, source=source, weight='weight')
    shortest_paths[source] = {"Lengths": lengths, "Paths": paths}

# Sonuçları yazdır
for source, data in shortest_paths.items():
    print(f"{nodes[source]} düğümünden diğer düğümlere olan en kısa mesafeler:")
    for target, distance in data["Lengths"].items():
        if source != target:  # Aynı düğümü atla
            print(f"  {nodes[target]}: Mesafe = {distance}, Yol = {data['Paths'][target]}")
    print()


# 4.c
# Verilen bir köşe numarasından başlatarak DFS ve BFS dolaşımı
start_node = 1  # Başlangıç düğümü (örneğin: John)

# DFS dolaşma
dfs_path = list(nx.dfs_edges(graph, source=start_node))
print(f"DFS ile {nodes[start_node]} düğümünden başlayarak dolaşılan yollar:")
for edge in dfs_path:
    print(f"{nodes[edge[0]]} -> {nodes[edge[1]]}")
print()

# BFS dolaşma
bfs_path = list(nx.bfs_edges(graph, source=start_node))
print(f"BFS ile {nodes[start_node]} düğümünden başlayarak dolaşılan yollar:")
for edge in bfs_path:
    print(f"{nodes[edge[0]]} -> {nodes[edge[1]]}")
print()

# 4.e
def find_nearest_doctor(graph, start_node):
    """
    Verilen bir başlangıç düğümünden en kısa sürede doktora ulaşma.
    """
    # Öncelik kuyruğu (mesafe, düğüm) için
    priority_queue = []
    heapq.heappush(priority_queue, (0, start_node))  # (Mesafe, Düğüm)
    
    # Ziyaret edilen düğümleri tut
    visited = set()
    
    while priority_queue:
        current_distance, current_node = heapq.heappop(priority_queue)
        
        # Eğer düğüm ziyaret edildiyse atla
        if current_node in visited:
            continue
        
        visited.add(current_node)
        
        # Eğer düğüm doktor ise döndür
        if "doktor" in graph.nodes[current_node]["label"].lower():
            return current_node, current_distance, len(visited)  # Doktor, mesafe, arama sayısı
        
        # Komşuları sıraya ekle
        for neighbor, attributes in graph[current_node].items():
            if neighbor not in visited:
                weight = attributes['weight']
                heapq.heappush(priority_queue, (current_distance + weight, neighbor))
    
    return None, float('inf'), len(visited)  # Doktora ulaşılamazsa

# Örnek kullanım
start_node = 1  # Başlangıç düğümü (örneğin John)

# Doktor bul
doctor, distance, search_attempts = find_nearest_doctor(graph, start_node)
if doctor:
    print(f"En yakın doktor: {nodes[doctor]} (Mesafe: {distance}, Arama Sayısı: {search_attempts})")
else:
    print(f"Hiçbir doktora ulaşılamadı. Toplam denenen düğüm sayısı: {search_attempts}")
