import React, { useState } from "react";
import styled from "styled-components";

const MenuContainer = styled.div`
  display: flex;
  max-width: 1440px;
  margin: 0 auto;
  padding: 40px 20px;

  @media (max-width: 768px) {
    flex-direction: column;
    padding: 20px;
  }
`;

const SidebarContainer = styled.div`
  width: 240px;
  padding-right: 24px;

  @media (max-width: 768px) {
    width: 100%;
    padding-right: 0;
    margin-bottom: 24px;
  }
`;

const MenuTitle = styled.h1`
  font-size: 1.5rem;
  margin-bottom: 20px;
  font-weight: 700;
`;

const CategoryList = styled.ul`
  list-style: none;
  padding: 0;
  margin: 0;
`;

const CategoryItem = styled.li`
  margin-bottom: 12px;

  a {
    color: ${(props) =>
      props.active ? "var(--starbucks-green)" : "rgba(0, 0, 0, 0.87)"};
    font-weight: ${(props) => (props.active ? "700" : "400")};
    display: block;
    padding: 8px 0;
    position: relative;

    &:hover {
      color: var(--starbucks-green);
    }

    &::after {
      content: "";
      position: absolute;
      bottom: 0;
      left: 0;
      width: ${(props) => (props.active ? "30px" : "0")};
      height: 2px;
      background-color: var(--starbucks-green);
      transition: width 0.3s ease;
    }

    &:hover::after {
      width: 30px;
    }
  }
`;

const MenuContentContainer = styled.div`
  flex: 1;
`;

const MenuHeader = styled.div`
  margin-bottom: 32px;
`;

const MenuDescription = styled.p`
  color: rgba(0, 0, 0, 0.7);
  font-size: 1rem;
  line-height: 1.6;
  max-width: 700px;
`;

const ProductGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 24px;

  @media (max-width: 480px) {
    grid-template-columns: repeat(2, 1fr);
    gap: 16px;
  }
`;

const ProductCard = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  cursor: pointer;
  transition: transform 0.3s ease;

  &:hover {
    transform: translateY(-5px);
  }
`;

const ProductImage = styled.div`
  width: 135px;
  height: 135px;
  border-radius: 50%;
  overflow: hidden;
  margin-bottom: 16px;

  img {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
`;

const ProductName = styled.h3`
  font-size: 1rem;
  margin: 0;
  font-weight: 600;
  color: rgba(0, 0, 0, 0.87);
`;

// Mock data
const categories = [
  { id: "drinks", name: "Drinks", active: true },
  { id: "food", name: "Food", active: false },
  { id: "at-home-coffee", name: "At Home Coffee", active: false },
  { id: "merchandise", name: "Merchandise", active: false },
  { id: "gift-cards", name: "Gift Cards", active: false },
];

const subcategories = [
  { id: "hot-coffees", name: "Hot Coffees", category: "drinks", active: true },
  { id: "hot-teas", name: "Hot Teas", category: "drinks", active: false },
  { id: "hot-drinks", name: "Hot Drinks", category: "drinks", active: false },
  {
    id: "frappuccino-blended-beverages",
    name: "Frappuccino® Blended Beverages",
    category: "drinks",
    active: false,
  },
  {
    id: "cold-coffees",
    name: "Cold Coffees",
    category: "drinks",
    active: false,
  },
  { id: "iced-teas", name: "Iced Teas", category: "drinks", active: false },
  { id: "cold-drinks", name: "Cold Drinks", category: "drinks", active: false },
];

const products = [
  {
    id: 1,
    name: "Caffè Americano",
    image:
      "https://globalassets.starbucks.com/assets/f12bc8af498d45ed92c5d6f1dac64062.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 2,
    name: "Blonde Roast",
    image:
      "https://globalassets.starbucks.com/assets/abb4f97948374050aec84784919c7def.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 3,
    name: "Caffè Misto",
    image:
      "https://globalassets.starbucks.com/assets/d668acbc691b47249548a3eeac449916.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 4,
    name: "Featured Dark Roast",
    image:
      "https://globalassets.starbucks.com/assets/0279f9c5fa5941d2a65dd7e5e4ccabc5.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 5,
    name: "Pike Place® Roast",
    image:
      "https://globalassets.starbucks.com/assets/dde7c4dbbe3e454891a133a0a68b94fb.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 6,
    name: "Decaf Pike Place® Roast",
    image:
      "https://globalassets.starbucks.com/assets/51bf549cd8e9434fa171696f35d24425.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 7,
    name: "Cappuccino",
    image:
      "https://globalassets.starbucks.com/assets/5c515339667943ce84dc56effdf5fc1b.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 8,
    name: "Flat White",
    image:
      "https://globalassets.starbucks.com/assets/fedee22e49724cd09fbcc7ee2e567377.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 9,
    name: "Honey Almondmilk Flat White",
    image:
      "https://globalassets.starbucks.com/assets/77801559b72b4d72b7d1cc1b5d87134b.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 10,
    name: "Caffè Latte",
    image:
      "https://globalassets.starbucks.com/assets/b635f407bbcd49e7b8dd9119ce33f76e.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 11,
    name: "Cinnamon Dolce Latte",
    image:
      "https://globalassets.starbucks.com/assets/e9330b18ae524e69bdcbe97460d6f524.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
  {
    id: 12,
    name: "Starbucks Reserve® Latte",
    image:
      "https://globalassets.starbucks.com/assets/55083489399e435e81cf41d899259b79.jpg",
    category: "drinks",
    subcategory: "hot-coffees",
  },
];

function Menu() {
  const [activeCategory, setActiveCategory] = useState("drinks");
  const [activeSubcategory, setActiveSubcategory] = useState("hot-coffees");

  const handleCategoryClick = (categoryId) => {
    setActiveCategory(categoryId);
    // Set active subcategory to first subcategory in this category
    const firstSubcategory = subcategories.find(
      (sub) => sub.category === categoryId,
    );
    if (firstSubcategory) {
      setActiveSubcategory(firstSubcategory.id);
    }
  };

  const filteredSubcategories = subcategories.filter(
    (subcategory) => subcategory.category === activeCategory,
  );

  const filteredProducts = products.filter(
    (product) =>
      product.category === activeCategory &&
      product.subcategory === activeSubcategory,
  );

  return (
    <MenuContainer>
      <SidebarContainer>
        <MenuTitle>Menu</MenuTitle>

        <h2
          style={{ fontSize: "1rem", marginBottom: "12px", fontWeight: "700" }}
        >
          Drinks
        </h2>
        <CategoryList>
          {filteredSubcategories.map((subcategory) => (
            <CategoryItem
              key={subcategory.id}
              active={subcategory.id === activeSubcategory}
            >
              <a
                href="#"
                onClick={(e) => {
                  e.preventDefault();
                  setActiveSubcategory(subcategory.id);
                }}
              >
                {subcategory.name}
              </a>
            </CategoryItem>
          ))}
        </CategoryList>
      </SidebarContainer>

      <MenuContentContainer>
        <MenuHeader>
          <MenuTitle>
            {subcategories.find((sub) => sub.id === activeSubcategory)?.name ||
              ""}
          </MenuTitle>
          <MenuDescription>
            {activeSubcategory === "hot-coffees" &&
              "Smooth, bold coffee drinks to wake you up. Crafted with care and passion by our baristas using the finest beans sourced from around the world."}
          </MenuDescription>
        </MenuHeader>

        <ProductGrid>
          {filteredProducts.map((product) => (
            <ProductCard key={product.id}>
              <ProductImage>
                <img src={product.image} alt={product.name} />
              </ProductImage>
              <ProductName>{product.name}</ProductName>
            </ProductCard>
          ))}
        </ProductGrid>
      </MenuContentContainer>
    </MenuContainer>
  );
}

export default Menu;
