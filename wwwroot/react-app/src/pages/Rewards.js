import React, { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';

const RewardsContainer = styled.div`
  width: 100%;
`;

const HeroSection = styled.section`
  background-color: #1e3932;
  color: white;
  padding: 48px 20px;
  text-align: center;
`;

const HeroTitle = styled.h1`
  font-size: 2.8rem;
  margin-bottom: 24px;
  
  @media (max-width: 768px) {
    font-size: 2rem;
  }
`;

const HeroSubtitle = styled.p`
  font-size: 1.5rem;
  margin-bottom: 32px;
  max-width: 700px;
  margin-left: auto;
  margin-right: auto;
  
  @media (max-width: 768px) {
    font-size: 1.25rem;
  }
`;

const JoinButton = styled(Link)`
  display: inline-block;
  background-color: white;
  color: #1e3932;
  font-weight: 600;
  padding: 7px 16px;
  border-radius: 50px;
  font-size: 1rem;
  transition: all 0.3s ease;
  
  &:hover {
    background-color: rgba(255, 255, 255, 0.9);
    transform: translateY(-2px);
  }
`;

const GetStartedSection = styled.section`
  padding: 64px 20px;
  text-align: center;
  background-color: #f7f7f7;
`;

const SectionTitle = styled.h2`
  font-size: 2rem;
  margin-bottom: 48px;
  
  @media (max-width: 768px) {
    font-size: 1.8rem;
  }
`;

const StepsContainer = styled.div`
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  max-width: 1200px;
  margin: 0 auto;
  gap: 40px;
`;

const StepItem = styled.div`
  max-width: 300px;
  text-align: center;
`;

const StepNumber = styled.div`
  width: 48px;
  height: 48px;
  border-radius: 50%;
  background-color: #00754a;
  color: white;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 1.5rem;
  font-weight: 700;
  margin: 0 auto 16px;
`;

const StepTitle = styled.h3`
  font-size: 1.25rem;
  margin-bottom: 12px;
`;

const StepDescription = styled.p`
  color: rgba(0, 0, 0, 0.7);
  line-height: 1.6;
`;

const FavoritesSection = styled.section`
  padding: 64px 20px;
  text-align: center;
`;

const TabsContainer = styled.div`
  display: flex;
  justify-content: center;
  margin-bottom: 48px;
  border-bottom: 2px solid #e5e5e5;
  max-width: 800px;
  margin-left: auto;
  margin-right: auto;
`;

const Tab = styled.button`
  background: none;
  border: none;
  padding: 16px 24px;
  font-size: 1.25rem;
  font-weight: ${props => props.active ? '700' : '400'};
  color: ${props => props.active ? '#00754a' : 'rgba(0, 0, 0, 0.7)'};
  position: relative;
  cursor: pointer;
  outline: none;
  
  &::after {
    content: '';
    position: absolute;
    bottom: -2px;
    left: 0;
    width: ${props => props.active ? '100%' : '0'};
    height: 4px;
    background-color: #00754a;
    transition: width 0.3s ease;
  }
  
  &:hover::after {
    width: 100%;
  }
`;

const RewardContent = styled.div`
  display: flex;
  align-items: center;
  max-width: 1000px;
  margin: 0 auto;
  
  @media (max-width: 992px) {
    flex-direction: column;
  }
`;

const RewardImage = styled.div`
  flex: 1;
  padding: 20px;
  
  img {
    max-width: 100%;
    border-radius: 8px;
  }
`;

const RewardInfo = styled.div`
  flex: 1;
  padding: 20px;
  text-align: left;
`;

const RewardInfoTitle = styled.h3`
  font-size: 1.75rem;
  margin-bottom: 16px;
  
  @media (max-width: 768px) {
    font-size: 1.5rem;
  }
`;

const RewardInfoDescription = styled.p`
  color: rgba(0, 0, 0, 0.7);
  line-height: 1.7;
  font-size: 1.1rem;
  margin-bottom: 24px;
  
  @media (max-width: 768px) {
    font-size: 1rem;
  }
`;

const CalloutSection = styled.section`
  background-color: #f7f7f7;
  padding: 64px 20px;
  text-align: center;
`;

const CalloutContainer = styled.div`
  max-width: 700px;
  margin: 0 auto;
`;

const CalloutTitle = styled.h2`
  font-size: 2rem;
  margin-bottom: 24px;
`;

const CalloutDescription = styled.p`
  color: rgba(0, 0, 0, 0.7);
  line-height: 1.7;
  font-size: 1.25rem;
  margin-bottom: 32px;
`;

// Mock rewards data
const rewardsData = [
  {
    id: 25,
    title: 'Customize your drink',
    description: 'Make your drink just right with an extra espresso shot, nondairy milk or a dash of your favorite syrup.',
    image: 'https://www.starbucks.com/weblx/images/rewards/reward-tiers/025.png'
  },
  {
    id: 100,
    title: 'Brewed hot or iced coffee or tea, bakery item, or packaged snack',
    description: 'Treat yourself to a pastry, a packaged snack or a hot cup of coffee or tea. Or keep cool with an iced coffee or iced tea.',
    image: 'https://www.starbucks.com/weblx/images/rewards/reward-tiers/100.png'
  },
  {
    id: 200,
    title: 'Handcrafted drink or hot breakfast',
    description: 'Turn good mornings great with a delicious handcrafted drink of your choice, breakfast sandwich or oatmeal on us.',
    image: 'https://www.starbucks.com/weblx/images/rewards/reward-tiers/200.png'
  },
  {
    id: 300,
    title: 'Salad, sandwich, or protein box',
    description: 'Enjoy a well-deserved lunch on us. Choose from one of our savory protein boxes, sandwiches or salads.',
    image: 'https://www.starbucks.com/weblx/images/rewards/reward-tiers/300.png'
  },
  {
    id: 400,
    title: 'Select Starbucks merchandise',
    description: 'Take home a signature cup, drink tumbler or your choice of coffee merch up to $20.',
    image: 'https://www.starbucks.com/weblx/images/rewards/reward-tiers/400.png'
  }
];

function Rewards() {
  const [activeTab, setActiveTab] = useState(25);
  
  const handleTabClick = (tabId) => {
    setActiveTab(tabId);
  };
  
  const activeReward = rewardsData.find(reward => reward.id === activeTab);
  
  return (
    <RewardsContainer>
      <HeroSection>
        <HeroTitle>Starbucks® Rewards</HeroTitle>
        <HeroSubtitle>Join Starbucks Rewards to earn Stars for free food and drinks, get access to mobile ordering, exclusive offers and more.</HeroSubtitle>
        <JoinButton to="/join">Join now</JoinButton>
      </HeroSection>
      
      <GetStartedSection>
        <SectionTitle>Getting started is easy</SectionTitle>
        <StepsContainer>
          <StepItem>
            <StepNumber>1</StepNumber>
            <StepTitle>Create an account</StepTitle>
            <StepDescription>
              To get started, join now. You can also join in the app to get access to the full range of Starbucks® Rewards benefits.
            </StepDescription>
          </StepItem>
          
          <StepItem>
            <StepNumber>2</StepNumber>
            <StepTitle>Order and pay how you'd like</StepTitle>
            <StepDescription>
              Use cash, credit/debit card or save some time and pay right through the app. You'll collect Stars all ways.
            </StepDescription>
          </StepItem>
          
          <StepItem>
            <StepNumber>3</StepNumber>
            <StepTitle>Earn Stars, get Rewards</StepTitle>
            <StepDescription>
              As you earn Stars, you can redeem them for Rewards—like free food, drinks, and more.
            </StepDescription>
          </StepItem>
        </StepsContainer>
      </GetStartedSection>
      
      <FavoritesSection>
        <SectionTitle>Get your favorites for free</SectionTitle>
        
        <TabsContainer>
          {rewardsData.map(reward => (
            <Tab 
              key={reward.id} 
              active={activeTab === reward.id}
              onClick={() => handleTabClick(reward.id)}
            >
              {reward.id} ★
            </Tab>
          ))}
        </TabsContainer>
        
        {activeReward && (
          <RewardContent>
            <RewardImage>
              <img src={activeReward.image} alt={activeReward.title} />
            </RewardImage>
            <RewardInfo>
              <RewardInfoTitle>{activeReward.title}</RewardInfoTitle>
              <RewardInfoDescription>{activeReward.description}</RewardInfoDescription>
            </RewardInfo>
          </RewardContent>
        )}
      </FavoritesSection>
      
      <CalloutSection>
        <CalloutContainer>
          <CalloutTitle>Endless Extras</CalloutTitle>
          <CalloutDescription>
            Joining Starbucks® Rewards means unlocking access to exclusive benefits. Order and pay ahead on the app, get access to special offers, birthday Rewards, and more.
          </CalloutDescription>
          <JoinButton to="/join" style={{ backgroundColor: '#00754a', color: 'white' }}>Join now</JoinButton>
        </CalloutContainer>
      </CalloutSection>
    </RewardsContainer>
  );
}

export default Rewards; 