import networkx as nx

# Initializing a directed graph, a directed graph's edges have directions.
KG = nx.DiGraph()

# Add nodes, nodes represent entities in the graph like people, places or objects.
KG.add_node("J.K. Rowling", type="Author")
KG.add_node("George Orwell", type="Author")
KG.add_node("Jane Austen", type="Author")

KG.add_node("Harry Potter and the Sorcerer's Stone", type="Book")
KG.add_node("1984", type="Book")
KG.add_node("Pride and Prejudice", type="Book")
KG.add_node("Sense and Sensibility", type="Book")
KG.add_node("Emma", type="Book")

KG.add_node("Hugo Award", type="Award")
KG.add_node("Booker Prize", type="Award")
KG.add_node("Franz Kafka Prize", type="Award")

KG.add_node("Fantasy", type="Genre")
KG.add_node("Dystopian", type="Genre")
KG.add_node("Romantic Fiction", type="Genre")
KG.add_node("Classic", type="Genre")

# Add edges, they define relationships between nodes.
KG.add_edge("J.K. Rowling", "Harry Potter and the Sorcerer's Stone", relationship="author of")
KG.add_edge("George Orwell", "1984", relationship="author of")
KG.add_edge("Jane Austen", "Pride and Prejudice", relationship="author of")
KG.add_edge("Jane Austen", "Sense and Sensibility", relationship="author of")
KG.add_edge("Jane Austen", "Emma", relationship="author of")

KG.add_edge("Harry Potter and the Sorcerer's Stone", "Hugo Award", relationship="won")
KG.add_edge("1984", "Booker Prize", relationship="nominated for")
KG.add_edge("Pride and Prejudice", "Franz Kafka Prize", relationship="nominated for")

KG.add_edge("Harry Potter and the Sorcerer's Stone", "Fantasy", relationship="genre")
KG.add_edge("1984", "Dystopian", relationship="genre")
KG.add_edge("Pride and Prejudice", "Romantic Fiction", relationship="genre")
KG.add_edge("Sense and Sensibility", "Romantic Fiction", relationship="genre")
KG.add_edge("Emma", "Romantic Fiction", relationship="genre")
KG.add_edge("Pride and Prejudice", "Classic", relationship="genre")
KG.add_edge("Sense and Sensibility", "Classic", relationship="genre")
KG.add_edge("Emma", "Classic", relationship="genre")

# Query function to find books by a particular author
def find_books_by_author(KG, author_name):
    books = []
    for book in KG.nodes:
        if KG.nodes[book].get("type") == "Book":
            # Check if the edge is directed from author to book
            if KG.has_edge(author_name, book) and KG.get_edge_data(author_name, book).get("relationship") == "author of":
                books.append(book)
    return books

# Query function to find awards won by a particular book
def find_awards_for_book(KG, book_title):
    awards = []
    for award in KG.nodes:
        if KG.nodes[award].get("type") == "Award":
            # Check if the edge is directed from book to award
            if KG.has_edge(book_title, award) and KG.get_edge_data(book_title, award).get("relationship") == "won":
                awards.append(award)
    return awards

# Query function to find genres of a particular book
def find_genres_for_book(KG, book_title):
    genres = []
    for genre in KG.nodes:
        if KG.nodes[genre].get("type") == "Genre":
            # Check if the edge is directed from book to genre
            if KG.has_edge(book_title, genre) and KG.get_edge_data(book_title, genre).get("relationship") == "genre":
                genres.append(genre)
    return genres



# Example queries
author_name = "Jane Austen"
books_by_author = find_books_by_author(KG, author_name)
print(f"Books by {author_name}: {books_by_author}")

book_title = "Harry Potter and the Sorcerer's Stone"
awards_for_book = find_awards_for_book(KG, book_title)
print(f"Awards won by {book_title}: {awards_for_book}")

book_genre = "Emma"
book_genres = find_genres_for_book(KG, book_genre)
print(f"Genres for {book_genre}: {book_genres}")