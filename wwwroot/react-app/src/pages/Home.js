import React from 'react';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

const HomeContainer = styled.div`
  margin-top: 0;
`;

const PromotionSection = styled.section`
  display: flex;
  align-items: center;
  background-color: ${props => props.bgColor || '#fff'};
  color: ${props => props.textColor || '#000'};
  padding: 32px 0;

  @media (max-width: 992px) {
    flex-direction: ${props => props.reversed ? 'column-reverse' : 'column'};
    text-align: center;
  }
`;

const PromotionImage = styled.div`
  flex: 1;
  
  img {
    width: 100%;
    display: block;
  }
`;

const PromotionContent = styled.div`
  flex: 1;
  padding: 0 48px;
  max-width: 540px;
  margin: 0 auto;
  
  @media (max-width: 992px) {
    padding: 32px 20px;
  }
`;

const PromotionTitle = styled.h2`
  font-size: 2.4rem;
  margin-bottom: 24px;
  
  @media (max-width: 768px) {
    font-size: 1.8rem;
  }
`;

const PromotionText = styled.p`
  font-size: 1.25rem;
  margin-bottom: 32px;
  line-height: 1.7;
  
  @media (max-width: 768px) {
    font-size: 1rem;
  }
`;

const PromotionButton = styled(Link)`
  display: inline-block;
  border: 1px solid ${props => props.textColor || '#000'};
  border-radius: 50px;
  padding: 7px 16px;
  font-weight: 600;
  font-size: 16px;
  text-align: center;
  color: ${props => props.textColor || '#000'};
  transition: all 0.3s ease;
  
  &:hover {
    background-color: rgba(0, 0, 0, 0.06);
  }
`;

const RewardsSection = styled.section`
  background-color: #f7f7f7;
  text-align: center;
  padding: 48px 20px;
`;

const RewardsTitle = styled.h2`
  font-size: 2rem;
  margin-bottom: 24px;
`;

const RewardsText = styled.p`
  font-size: 1.25rem;
  max-width: 800px;
  margin: 0 auto 32px;
`;

// Mock data for promotions
const promotions = [
  {
    id: 1,
    title: "Sweet dreams are made of spring",
    text: "Celebrate the season with our new Iced Lavender Cream Oatmilk Latte and Lavender Crème Frappuccino® blended beverage.",
    cta: "Order now",
    image: "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-88168.jpg",
    link: "/menu",
    bgColor: "#ebc8ed",
    textColor: "#1e3932",
    reversed: false
  },
  {
    id: 2,
    title: "Crafted for comfort",
    text: "Our perfectly toasted egg & cheddar sandwich pairs well with your morning coffee.",
    cta: "Learn more",
    image: "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-88437.jpg",
    link: "/menu",
    bgColor: "#d4e9e2",
    textColor: "#1e3932",
    reversed: true
  },
  {
    id: 3,
    title: "Refresh on repeat",
    text: "Brighten your day with a Frozen Lemonade Starbucks Refreshers® beverage, a cheerful blend of lemonade and strawberry açaí, pineapple passionfruit or mango dragonfruit flavors.",
    cta: "Order now",
    image: "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-88037.jpg",
    link: "/menu",
    bgColor: "#ff9ed1",
    textColor: "#1e3932",
    reversed: false
  },
  {
    id: 4,
    title: "Rewards are pouring",
    text: "Join Starbucks® Rewards and start earning free drinks and food today.",
    cta: "Join now",
    image: "https://content-prod-live.cert.starbucks.com/binary/v2/asset/137-88028.jpg",
    link: "/rewards",
    bgColor: "#1e3932",
    textColor: "#ffffff",
    reversed: true
  }
];

function Home() {
  return (
    <HomeContainer>
      {promotions.map(promo => (
        <PromotionSection 
          key={promo.id} 
          bgColor={promo.bgColor} 
          textColor={promo.textColor}
          reversed={promo.reversed}
        >
          {!promo.reversed ? (
            <>
              <PromotionImage>
                <img src={promo.image} alt={promo.title} />
              </PromotionImage>
              <PromotionContent>
                <PromotionTitle>{promo.title}</PromotionTitle>
                <PromotionText>{promo.text}</PromotionText>
                <PromotionButton to={promo.link} textColor={promo.textColor}>
                  {promo.cta}
                </PromotionButton>
              </PromotionContent>
            </>
          ) : (
            <>
              <PromotionContent>
                <PromotionTitle>{promo.title}</PromotionTitle>
                <PromotionText>{promo.text}</PromotionText>
                <PromotionButton to={promo.link} textColor={promo.textColor}>
                  {promo.cta}
                </PromotionButton>
              </PromotionContent>
              <PromotionImage>
                <img src={promo.image} alt={promo.title} />
              </PromotionImage>
            </>
          )}
        </PromotionSection>
      ))}
      
      <RewardsSection>
        <RewardsTitle>Starbucks® Rewards</RewardsTitle>
        <RewardsText>
          Join Starbucks Rewards to earn Stars for free food and drinks, 
          get access to mobile ordering, exclusive offers and more.
        </RewardsText>
        <PromotionButton to="/rewards" textColor="#000">
          Join now
        </PromotionButton>
      </RewardsSection>
    </HomeContainer>
  );
}

export default Home; 